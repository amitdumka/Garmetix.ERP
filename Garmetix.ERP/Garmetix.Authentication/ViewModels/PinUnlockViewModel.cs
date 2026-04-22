using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Garmetix.Authentication.Services;
using Garmetix.Core.Interfaces;
using Garmetix.Core.ViewModels;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;

namespace Garmetix.Authentication.ViewModels
{
    public partial class PinUnlockViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string _pinEntry = "";
        [ObservableProperty] private string _message = "Enter your 4-digit PIN";
        [ObservableProperty] private Color _messageColor = Color.FromArgb("#64748B");

        [ObservableProperty] private bool _dot1;
        [ObservableProperty] private bool _dot2;
        [ObservableProperty] private bool _dot3;
        [ObservableProperty] private bool _dot4;

        public PinUnlockViewModel(IAuthService authService, INotificationService notification) : base(notification)
        {
            _authService = authService;
            Title = "Secure Unlock";
        }

        [RelayCommand]
        public async Task KeypressAsync(string digit)
        {
            Message = "Enter your 4-digit PIN";
            MessageColor = Color.FromArgb("#64748B");

            if (digit == "DEL")
            {
                if (PinEntry.Length > 0) PinEntry = PinEntry.Substring(0, PinEntry.Length - 1);
            }
            else if (PinEntry.Length < 4)
            {
                PinEntry += digit;
            }

            UpdateDots();

            if (PinEntry.Length == 4)
            {
                await ValidatePinAsync();
            }
        }

        private void UpdateDots()
        {
            Dot1 = PinEntry.Length >= 1;
            Dot2 = PinEntry.Length >= 2;
            Dot3 = PinEntry.Length >= 3;
            Dot4 = PinEntry.Length >= 4;
        }

        private async Task ValidatePinAsync()
        {
            IsBusy = true;
            bool isValid = await _authService.UnlockWithPinAsync(PinEntry);
            IsBusy = false;

            if (isValid)
            {
                Message = "Access Granted";
                MessageColor = Color.FromArgb("#10B981");
                await Task.Delay(300);
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                Message = "Incorrect PIN. Try again.";
                MessageColor = Color.FromArgb("#EF4444");
                PinEntry = "";
                UpdateDots();
            }
        }

        [RelayCommand]
        public async Task SwitchUserAsync()
        {
            _authService.Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}