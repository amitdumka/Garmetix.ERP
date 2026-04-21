using Garmetix.Authentication.ViewModels;

namespace Garmetix.Authentication.Views;

public partial class ChangePasswordPage : ContentPage
{
	public ChangePasswordPage(ChangePasswordViewModel viewModel)
    {
        InitializeComponent();

        // 2. Assign the injected ViewModel to the page's BindingContext
        BindingContext = viewModel;
    }

    // Optional: If you need to trigger a ViewModel method right when the page opens
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Example: Clear the password field every time the page appears
        if (BindingContext is ChangePasswordViewModel vm)
        {
            vm.OldPassword = string.Empty;
            vm.NewPassword = string.Empty;
            vm.ConfirmPassword = string.Empty;
        }
    }
}