using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using Garmetix.Core.Interfaces;

namespace Garmetix.Core.ViewModels
{
    public partial class BaseViewModel : ObservableObject
    {
        protected readonly INotificationService _notificationService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        private bool _isBusy;

        [ObservableProperty]
        private string _title;

        public bool IsNotBusy => !IsBusy;

        // Constructor injection for the shared service
        public BaseViewModel(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        /// <summary>
        /// Wraps any async operation in a global try/catch, manages IsBusy, and shows standard errors.
        /// </summary>
        protected async Task ExecuteSafeAsync(Func<Task> operation, string errorTitle = "Operation Failed")
        {
            if (IsBusy) return; // Prevent double-clicking

            IsBusy = true;
            try
            {
                await operation();
            }
            catch (Exception ex)
            {
                // Log exception to a file or cloud telemetry here (e.g., AppCenter/Sentry)
                await _notificationService.ShowErrorAsync(errorTitle, ex.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}