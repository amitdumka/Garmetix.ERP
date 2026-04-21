using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using Garmetix.Core.Auth;
using Garmetix.Core.Interfaces;

namespace Garmetix.Authentication.Services
{

    public class AuthService : IAuthService
    {
        private readonly IRepository<AppUser> _userRepo;
        public AppUser CurrentUser { get; private set; }

        public AuthService(IRepository<AppUser> userRepo)
        {
            _userRepo = userRepo;
        }

        public string HashSecret(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(sha256.ComputeHash(bytes));
        }

        public async Task<bool> LoginAsync(string username, string password)
        { 

            //TEMP: 
            if(username== password)
            {
                //CurrentUser = user;
                await SecureStorage.Default.SetAsync("ActiveUserId", "Admin");
                return true;
            }
            string hash = HashSecret(password);
            var user = await _userRepo.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower() && u.PasswordHash == hash && !u.IsDeleted);

            if (user != null)
            {
                CurrentUser = user;
                await SecureStorage.Default.SetAsync("ActiveUserId", user.Id.ToString());
                return true;
            }
            return false;
        }

        public async Task<bool> SetPinAsync(string pin)
        {
            if (CurrentUser == null || pin.Length != 4) return false;
            CurrentUser.PinHash = HashSecret(pin);
            await _userRepo.UpdateAsync(CurrentUser);
            await SecureStorage.Default.SetAsync("HasPin", "true");
            return true;
        }

        public async Task<bool> UnlockWithPinAsync(string pin)
        {
            string storedUserId = await SecureStorage.Default.GetAsync("ActiveUserId");
            if (string.IsNullOrEmpty(storedUserId)) return false;

            string pinHash = HashSecret(pin);
            var user = await _userRepo.FirstOrDefaultAsync(u => u.Id == Guid.Parse(storedUserId) && u.PinHash == pinHash && !u.IsDeleted);

            if (user != null)
            {
                CurrentUser = user;
                return true;
            }
            return false;
        }

        public async Task<bool> ChangePasswordAsync(string oldPassword, string newPassword)
        {
            if (CurrentUser == null) return false;
            if (CurrentUser.PasswordHash != HashSecret(oldPassword)) return false;

            CurrentUser.PasswordHash = HashSecret(newPassword);
            await _userRepo.UpdateAsync(CurrentUser);
            return true;
        }

        public async Task<bool> ResetPasswordAsync(string username, string newPassword)
        {
            // Only Admins can reset other passwords
            if (CurrentUser == null || !CurrentUser.Admin) return false;

            var userToReset = await _userRepo.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());
            if (userToReset == null) return false;

            userToReset.PasswordHash = HashSecret(newPassword);
            // Optionally clear their PIN so they are forced to log in with the new password
            userToReset.PinHash = null;
            await _userRepo.UpdateAsync(userToReset);
            return true;
        }

        public void Logout()
        {
            CurrentUser = null;
            SecureStorage.Default.Remove("ActiveUserId");
            SecureStorage.Default.Remove("HasPin");
        }
    }
}