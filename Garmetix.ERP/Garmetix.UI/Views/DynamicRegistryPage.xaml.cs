using Microsoft.Maui.Controls;
using Garmetix.UI.ViewModels;

namespace Garmetix.UI.Views
{
    public partial class DynamicRegistryPage : ContentPage
    {
        public DynamicRegistryPage(DynamicRegistryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Auto-refresh the list whenever the user returns to this page
            if (BindingContext is DynamicRegistryViewModel vm)
            {
                _ = vm.LoadDataAsync();
            }
        }
    }
}