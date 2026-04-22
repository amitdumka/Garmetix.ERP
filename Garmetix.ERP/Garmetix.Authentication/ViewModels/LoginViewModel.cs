using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core.ViewModels;
using Garmetix.Authentication.Services;
using Garmetix.Core.Interfaces;

namespace Garmetix.Authentication.ViewModels
{
    public partial class LoginViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string _userName;
        [ObservableProperty] private string _password;
        [ObservableProperty] private string _errorMessage;

        public LoginViewModel(IAuthService authService, INotificationService notification): base(notification)
        {
            _authService = authService;
            Title = "System Login";
        }

        [RelayCommand]
        public async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Credentials required.";
                return;
            }

            IsBusy = true;
            ErrorMessage = string.Empty;

            bool success = await _authService.LoginAsync(UserName, Password);
            IsBusy = false;

            if (success)
            {
                // Go to Dashboard (or PIN Setup if it's their first time)
                await Shell.Current.GoToAsync("//DashboardPage");
            }
            else
            {
                ErrorMessage = "Invalid credentials.";
            }
        }
    }
}