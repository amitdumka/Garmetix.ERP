namespace Garmetix.Core.Interfaces
{
    public interface INotificationService
    {
        Task ShowSuccessAsync(string message);
        Task ShowErrorAsync(string title, string exceptionMessage);
        Task ShowWarningAsync(string message);
        Task<bool> ShowConfirmAsync(string title, string message);
    }
}