using Microsoft.Maui.Controls;
using Garmetix.UI.ViewModels;

namespace Garmetix.UI.Views
{
    public partial class DynamicRegistryPage : ContentPage
    {

        // Add this property to catch the string from AppShell.xaml
        public string EntityType
        {
            get => (BindingContext as DynamicRegistryViewModel)?.EntityType;
            set
            {
                if (BindingContext is DynamicRegistryViewModel vm)
                {
                    vm.EntityType = value; // Pass it straight to the ViewModel!
                }
            }
        }

        // 1. ADD THIS: XAML needs this empty constructor to build the UI from AppShell
        public DynamicRegistryPage()
        {
            InitializeComponent();

            // Safely grab the ViewModel from MAUI's Dependency Injection system
            var serviceProvider = IPlatformApplication.Current.Services;
            BindingContext = serviceProvider.GetService<DynamicRegistryViewModel>();
        }
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