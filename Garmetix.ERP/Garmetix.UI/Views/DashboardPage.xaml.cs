using Microsoft.Maui.Controls;
using Garmetix.UI.ViewModels;

namespace Garmetix.UI.Views
{
    public partial class DashboardPage : ContentPage
    {
        public DashboardPage(DashboardViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            // Refresh dashboard numbers every time the user looks at it
            if (BindingContext is DashboardViewModel vm)
            {
                _ = vm.LoadMetricsAsync();
            }
        }
    }
}