using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core;
using Garmetix.Core.ViewModels;
using Garmetix.Core.Models.Accounting;
using Garmetix.Core.Interfaces;

namespace Garmetix.UI.ViewModels.Accounting
{
    [QueryProperty(nameof(VoucherId), "VoucherId")]
    public partial class VoucherEntryViewModel : BaseViewModel
    {
        private readonly IRepository<Voucher> _voucherRepo;
        private readonly IRepository<Party> _partyRepo;
        private readonly IRepository<BankAccount> _bankRepo;

        [ObservableProperty] private string _voucherId;
        [ObservableProperty] private Voucher _currentVoucher;
        
        [ObservableProperty] private ObservableCollection<Party> _availableParties = new();
        [ObservableProperty] private ObservableCollection<BankAccount> _availableBanks = new();

        public Array VoucherTypes => Enum.GetValues(typeof(VoucherType));
        public Array PaymentModes => Enum.GetValues(typeof(PaymentMode));

        // Interceptors for UI manipulation
        [ObservableProperty] private bool _isBankMode;
        [ObservableProperty] private PaymentMode _selectedPaymentMode = PaymentMode.Cash;
        [ObservableProperty] private BankAccount _selectedBank;

        public VoucherEntryViewModel(IRepository<Voucher> voucherRepo, IRepository<Party> partyRepo, IRepository<BankAccount> bankRepo)
        {
            _voucherRepo = voucherRepo;
            _partyRepo = partyRepo;
            _bankRepo = bankRepo;
            
            Title = "New Voucher Entry";
            CurrentVoucher = new Voucher { PartyName="", VoucherNumber = "VCH-" + DateTime.Now.ToString("yyMMddHHmm"), OnDate = DateTime.Now , Particulars=""};
            _ = LoadDependenciesAsync();
        }

        partial void OnSelectedPaymentModeChanged(PaymentMode value)
        {
            if (CurrentVoucher != null) CurrentVoucher.PaymentMode = value;
            IsBankMode = value != PaymentMode.Cash;
            if (!IsBankMode)
            {
                CurrentVoucher.AccountNumber = null;
                CurrentVoucher.PaymentDetails = string.Empty;
                SelectedBank = null;
            }
        }

        partial void OnSelectedBankChanged(BankAccount value)
        {
            if (CurrentVoucher != null) CurrentVoucher.AccountNumber = value?.Id;
        }

        private async Task LoadDependenciesAsync()
        {
            AvailableParties = new ObservableCollection<Party>(await _partyRepo.GetAllAsync());
            AvailableBanks = new ObservableCollection<BankAccount>(await _bankRepo.GetAllAsync());
        }

        [RelayCommand]
        public async Task SaveAsync()
        {
            if (string.IsNullOrWhiteSpace(CurrentVoucher.PartyName))
            {
                await Application.Current.MainPage.DisplayAlert("Validation", "Party Name is required.", "OK");
                return;
            }

            IsBusy = true;
            try
            {
                // Match Party to DB if exists
                var matchedParty = AvailableParties.FirstOrDefault(p => p.Name.Equals(CurrentVoucher.PartyName, StringComparison.OrdinalIgnoreCase));
                if (matchedParty != null)
                {
                    CurrentVoucher.PartyId = matchedParty.Id;
                    CurrentVoucher.IsParty = true;
                }

                if (string.IsNullOrEmpty(VoucherId)) await _voucherRepo.AddAsync(CurrentVoucher);
                else await _voucherRepo.UpdateAsync(CurrentVoucher);

                await Shell.Current.GoToAsync("..");
            }
            finally { IsBusy = false; }
        }
    }
}