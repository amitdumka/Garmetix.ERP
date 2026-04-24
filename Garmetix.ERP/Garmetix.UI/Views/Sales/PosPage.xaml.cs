using Microsoft.Maui.Controls;
using Garmetix.UI.ViewModels.Sales;

namespace Garmetix.UI.Views.Sales
{
    public partial class PosPage : ContentPage
    {
        public PosPage(PosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (BindingContext is PosViewModel vm)
            {
                _ = vm.LoadProductsAsync();
            }
        }
    }
}