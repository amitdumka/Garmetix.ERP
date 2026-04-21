using Microsoft.Extensions.DependencyInjection;
using Garmetix.Authentication.Services;
using Garmetix.Authentication.ViewModels;
using Garmetix.Authentication.Views;

namespace Garmetix.Authentication
{
    public static class AuthExtensions
    {

        public static MauiAppBuilder UseGarmetixAuthentication(this MauiAppBuilder builder)
        {
            builder.Services.AddGarmetixAuthentication();
            return builder;
        }
        public static IServiceCollection AddGarmetixAuthentication(this IServiceCollection services)
        {
            // Register Core Security Service
            services.AddSingleton<IAuthService, AuthService>();

            // Register ViewModels
            services.AddTransient<LoginViewModel>();
            services.AddTransient<PinUnlockViewModel>(); // Assuming you copy over your Numpad VM
            services.AddTransient<ChangePasswordViewModel>();
            services.AddTransient<ResetPasswordViewModel>();

            // Register Views
            services.AddTransient<LoginPage>();
            services.AddTransient<PinUnlockPage>();
            services.AddTransient<ChangePasswordPage>();
            services.AddTransient<ResetPasswordPage>();

            return services;
        }
    }
}