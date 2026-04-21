using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core;
using Garmetix.Core.ViewModels;
using Garmetix.Core.Models.Sales;
using Garmetix.Core.Interfaces;

namespace Garmetix.UI.ViewModels.Sales
{
    public partial class PosViewModel : BaseViewModel
    {
        private readonly IRepository<Invoice> _invoiceRepo;
        private readonly IRepository<InvoiceItem> _invoiceItemRepo;
        private readonly IRepository<Stock> _stockRepo;
        private readonly IRepository<Product> _productRepo;

        [ObservableProperty] private Invoice _currentBill;
        [ObservableProperty] private ObservableCollection<InvoiceItem> _cartItems = new();
        [ObservableProperty] private ObservableCollection<Product> _availableProducts = new();
        
        [ObservableProperty] private Product _selectedProduct;

        public Array PaymentModes => Enum.GetValues(typeof(PaymentMode));

        public PosViewModel(IRepository<Invoice> invoiceRepo, IRepository<InvoiceItem> invoiceItemRepo, IRepository<Stock> stockRepo, IRepository<Product> productRepo)
        {
            _invoiceRepo = invoiceRepo;
            _invoiceItemRepo = invoiceItemRepo;
            _stockRepo = stockRepo;
            _productRepo = productRepo;

            Title = "Retail Point of Sale";
            ResetPOS();
            _ = LoadProductsAsync();
        }

        private void ResetPOS()
        {
            CurrentBill = new Invoice { InvoiceNo = "INV-" + DateTime.Now.ToString("yyMMddHHmmss") };
            CartItems.Clear();
            SelectedProduct = null;
        }

        private async Task LoadProductsAsync()
        {
            var prods = await _productRepo.GetAllAsync();
            AvailableProducts = new ObservableCollection<Product>(prods);
        }

        partial void OnSelectedProductChanged(Product value)
        {
            if (value != null)
            {
                // Add to cart
                var existingItem = CartItems.FirstOrDefault(i => i.ProductId == value.Id);
                if (existingItem != null)
                {
                    existingItem.Quantity++;
                    existingItem.TaxAmount = (existingItem.Quantity * existingItem.Rate) * (value.TaxPercentage / 100);
                    existingItem.TotalAmount = (existingItem.Quantity * existingItem.Rate) + existingItem.TaxAmount;
                }
                else
                {
                    CartItems.Add(new InvoiceItem
                    {
                        ProductId = value.Id,
                        ProductName = value.Name,
                        Rate = value.SellingPrice,
                        Quantity = 1,
                        TaxAmount = value.SellingPrice * (value.TaxPercentage / 100),
                        TotalAmount = value.SellingPrice + (value.SellingPrice * (value.TaxPercentage / 100))
                    });
                }

                CalculateTotals();
                SelectedProduct = null; // Reset search bar
            }
        }

        private void CalculateTotals()
        {
            CurrentBill.SubTotal = CartItems.Sum(i => i.Rate * i.Quantity);
            CurrentBill.TotalTax = CartItems.Sum(i => i.TaxAmount);
            CurrentBill.GrandTotal = CurrentBill.SubTotal + CurrentBill.TotalTax;
            OnPropertyChanged(nameof(CurrentBill)); // Refresh UI
        }

        [RelayCommand]
        public async Task CheckoutAsync()
        {
            if (CartItems.Count == 0) return;
            IsBusy = true;

            try
            {
                // 1. EF Core Atomic Transaction
                await _invoiceRepo.ExecuteTransactionAsync(async () =>
                {
                    // A. Save Main Invoice
                    await _invoiceRepo.AddAsync(CurrentBill);

                    foreach (var item in CartItems)
                    {
                        // B. Save Line Items
                        item.InvoiceId = CurrentBill.Id;
                        await _invoiceItemRepo.AddAsync(item);

                        // C. Deduct Inventory Securely
                        var stock = await _stockRepo.FirstOrDefaultAsync(s => s.ProductId == item.ProductId);
                        if (stock != null)
                        {
                            stock.CurrentQty -= item.Quantity;
                            await _stockRepo.UpdateAsync(stock);
                        }
                    }
                });

                await Application.Current.MainPage.DisplayAlert("Success", "Payment Collected & Invoice Saved", "OK");
                
                // Print PDF logic goes here...

                ResetPOS();
            }
            catch (Exception ex) { await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK"); }
            finally { IsBusy = false; }
        }
    }
}