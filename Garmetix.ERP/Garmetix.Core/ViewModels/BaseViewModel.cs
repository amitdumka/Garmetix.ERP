using CommunityToolkit.Mvvm.ComponentModel;

namespace Garmetix.Core.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _title;

        // Automatically flips to false when IsBusy is true. 
        // Great for binding to the "IsEnabled" property of a Save button to prevent double-clicking!
        public bool IsNotBusy => !IsBusy; 
    }
}