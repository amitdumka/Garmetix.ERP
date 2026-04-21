using Garmetix.Authentication.ViewModels;

namespace Garmetix.Authentication.Views;

public partial class ResetPasswordPage : ContentPage
{
    public ResetPasswordPage(ResetPasswordViewModel viewModel)
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
        if (BindingContext is ResetPasswordViewModel vm)
        {
            vm.NewPassword = string.Empty;
        }
    }
}