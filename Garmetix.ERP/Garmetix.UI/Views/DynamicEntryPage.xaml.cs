using Microsoft.Maui.Controls;
using Garmetix.UI.ViewModels;

namespace Garmetix.UI.Views
{
    public partial class DynamicEntryPage : ContentPage
    {
        public DynamicEntryPage(DynamicEntryViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}