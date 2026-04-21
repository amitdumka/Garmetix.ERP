using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using Garmetix.Core.Interfaces;

namespace Garmetix.UI.Services
{
    public class AppNotificationService : INotificationService
    {
        public Task ShowSuccessAsync(string message)
        {
            // Can be upgraded to a Toast/Snackbar later
            return Application.Current.MainPage.DisplayAlert("Success", message, "OK");
        }

        public Task ShowErrorAsync(string title, string exceptionMessage)
        {
            return Application.Current.MainPage.DisplayAlert($"Error: {title}", exceptionMessage, "OK");
        }

        public Task ShowWarningAsync(string message)
        {
            return Application.Current.MainPage.DisplayAlert("Warning", message, "OK");
        }

        public Task<bool> ShowConfirmAsync(string title, string message)
        {
            return Application.Current.MainPage.DisplayAlert(title, message, "Yes", "No");
        }
    }
}