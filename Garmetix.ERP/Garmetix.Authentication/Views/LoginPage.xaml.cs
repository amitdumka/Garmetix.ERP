using Garmetix.Authentication.ViewModels;

namespace Garmetix.Authentication.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
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
        if (BindingContext is LoginViewModel vm)
        {
            vm.Password = string.Empty;
        }
    }
}