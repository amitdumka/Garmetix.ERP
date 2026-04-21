using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using Garmetix.Core.ViewModels;
using Garmetix.Authentication.Services;

namespace Garmetix.Authentication.ViewModels
{
    public partial class ResetPasswordViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string _targetUsername;
        [ObservableProperty] private string _newPassword;

        public ResetPasswordViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Admin: Reset Staff Password";
        }

        [RelayCommand]
        public async Task ResetPasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(TargetUsername) || string.IsNullOrWhiteSpace(NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert("Validation", "Username and New Password are required.", "OK");
                return;
            }

            IsBusy = true;
            bool success = await _authService.ResetPasswordAsync(TargetUsername, NewPassword);
            IsBusy = false;

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", $"Password for {TargetUsername} has been reset. Their PIN was cleared.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Action failed. User not found or you lack Admin permissions.", "OK");
            }
        }
    }
}