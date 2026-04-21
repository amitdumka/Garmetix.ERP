using Garmetix.Authentication.ViewModels;

namespace Garmetix.Authentication.Views;

public partial class PinUnlockPage : ContentPage
{
	
    public PinUnlockPage(PinUnlockViewModel viewModel)
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
        if (BindingContext is PinUnlockViewModel vm)
        {
            vm.PinEntry = string.Empty;
            vm.Dot2 = false;
            vm.Dot3 = false;
            vm.Dot4 = false;
            vm.Dot1 = false;
        }
    }
}