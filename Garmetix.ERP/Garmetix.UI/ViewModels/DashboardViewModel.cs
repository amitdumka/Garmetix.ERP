using System;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Garmetix.Core.ViewModels;
using Garmetix.Core.Interfaces;
using Garmetix.Core.Models.Accounting;

namespace Garmetix.UI.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IRepository<Voucher> _voucherRepo;
        private readonly IRepository<BankAccount> _bankRepo;

        // Dashboard Metrics
        [ObservableProperty] private decimal _todaysCollections;
        [ObservableProperty] private decimal _todaysExpenses;
        [ObservableProperty] private decimal _totalBankBalance;

        public DashboardViewModel(IRepository<Voucher> voucherRepo, IRepository<BankAccount> bankRepo)
        {
            _voucherRepo = voucherRepo;
            _bankRepo = bankRepo;
            Title = "Executive Dashboard";
        }

        public async Task LoadMetricsAsync()
        {
            if (IsBusy) return;
            IsBusy = true;

            try
            {
                DateTime today = DateTime.Today;

                // 1. Calculate Today's Vouchers
                var vouchers = await _voucherRepo.GetAllAsync();
                
                TodaysCollections = vouchers
                    .Where(v => v.VoucherType == Core.VoucherType.Receipt && v.OnDate.Date == today)
                    .Sum(v => v.Amount);

                TodaysExpenses = vouchers
                    .Where(v => v.VoucherType == Core.VoucherType.Expense && v.OnDate.Date == today)
                    .Sum(v => v.Amount);

                // 2. Calculate Total Bank Balance
                var accounts = await _bankRepo.GetAllAsync();
                TotalBankBalance = accounts.Sum(a => a.ClosingBalance);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        public async Task QuickActionAsync(string route)
        {
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync(route);
        }
    }
}