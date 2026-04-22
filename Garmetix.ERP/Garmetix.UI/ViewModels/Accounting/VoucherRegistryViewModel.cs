using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core.ViewModels;
using Garmetix.Core.Models.Accounting;
using Garmetix.Core.Interfaces;

namespace Garmetix.UI.ViewModels.Accounting
{
    public partial class VoucherRegistryViewModel : BaseViewModel
    {
        private readonly IRepository<Voucher> _voucherRepo;

        [ObservableProperty] private string _searchText;
        private System.Collections.Generic.List<Voucher> _allVouchers = new();
        [ObservableProperty] private ObservableCollection<Voucher> _filteredVouchers = new();

        public VoucherRegistryViewModel(IRepository<Voucher> voucherRepo, INotificationService notification) : base(notification)
        {
            _voucherRepo = voucherRepo;
            Title = "Aadwika Fashion | Vouchers";
        }

        public async Task LoadDataAsync()
        {
            if (IsBusy) return;
            IsBusy = true;
            try
            {
                var data = await _voucherRepo.GetAllAsync();
                _allVouchers = data.OrderByDescending(v => v.OnDate).ToList();
                ApplyFilters();
            }
            finally { IsBusy = false; }
        }

        partial void OnSearchTextChanged(string value) => ApplyFilters();

        private void ApplyFilters()
        {
            var query = _allVouchers.AsEnumerable();
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                var search = SearchText.ToLower();
                query = query.Where(v => v.VoucherNumber.ToLower().Contains(search) || v.PartyName.ToLower().Contains(search));
            }
            FilteredVouchers = new ObservableCollection<Voucher>(query.ToList());
        }

        [RelayCommand]
        public async Task AddNewVoucherAsync() => await Shell.Current.GoToAsync("VoucherEntryPage");

        [RelayCommand]
        public async Task ActionMenuAsync(Voucher voucher)
        {
            if (voucher == null) return;
            string action = await Application.Current.MainPage.DisplayActionSheet($"{voucher.VoucherNumber}", "Cancel", "Delete", "Edit Voucher");

            if (action == "Edit Voucher") await Shell.Current.GoToAsync($"VoucherEntryPage?VoucherId={voucher.Id}");
            else if (action == "Delete")
            {
                if (await Application.Current.MainPage.DisplayAlert("Delete", $"Delete {voucher.VoucherNumber}?", "Yes", "No"))
                {
                    await _voucherRepo.DeleteAsync(voucher); // Soft deletes via EF Core!
                    await LoadDataAsync();
                }
            }
        }
    }
}