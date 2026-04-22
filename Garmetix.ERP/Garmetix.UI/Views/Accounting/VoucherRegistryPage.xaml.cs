using Garmetix.UI.ViewModels.Accounting;

namespace Garmetix.UI.Views.Accounting;
public partial class VoucherRegistryPage : ContentPage
{
    public VoucherRegistryPage(VoucherRegistryViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}