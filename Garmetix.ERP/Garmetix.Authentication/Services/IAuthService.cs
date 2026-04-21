using Garmetix.Core.Auth;

namespace Garmetix.Authentication.Services
{
    public interface IAuthService
    {
        AppUser CurrentUser { get; }
        string HashSecret(string input);
        Task<bool> LoginAsync(string username, string password);
        Task<bool> SetPinAsync(string pin);
        Task<bool> UnlockWithPinAsync(string pin);
        Task<bool> ChangePasswordAsync(string oldPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string username, string newPassword); // For Admins
        void Logout();
    }
}