using Garmetix.UI.ViewModels;
using Garmetix.UI.ViewModels.Accounting;

namespace Garmetix.UI.Views.Accounting;

public partial class VoucherRegistryPage : ContentPage
{
    public VoucherRegistryPage(VoucherRegistryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }


    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Auto-refresh the list whenever the user returns to this page
        if (BindingContext is VoucherRegistryViewModel vm)
        {
            _ = vm.LoadDataAsync();
        }
    }
}