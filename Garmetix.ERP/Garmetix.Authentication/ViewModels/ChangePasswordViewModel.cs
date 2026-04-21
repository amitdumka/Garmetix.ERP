using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Garmetix.Core.ViewModels;
using Garmetix.Authentication.Services;
namespace Garmetix.Authentication.ViewModels
{
    public partial class ChangePasswordViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string _oldPassword;
        [ObservableProperty] private string _newPassword;
        [ObservableProperty] private string _confirmPassword;

        public ChangePasswordViewModel(IAuthService authService)
        {
            _authService = authService;
            Title = "Change Password";
        }

        [RelayCommand]
        public async Task ChangePasswordAsync()
        {
            if (NewPassword != ConfirmPassword)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "New passwords do not match.", "OK");
                return;
            }

            IsBusy = true;
            bool success = await _authService.ChangePasswordAsync(OldPassword, NewPassword);
            IsBusy = false;

            if (success)
            {
                await Application.Current.MainPage.DisplayAlert("Success", "Password updated successfully.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrect old password.", "OK");
            }
        }
    }
}
