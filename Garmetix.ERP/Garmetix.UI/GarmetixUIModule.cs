using Garmetix.Core.Interfaces;
using Garmetix.UI.Services;
using Garmetix.UI.ViewModels;
using Garmetix.UI.Views;

namespace Garmetix.UI
{
    // All the code in this file is included in all platforms.
    public static class GarmetixUIModule
    {

        public static void EnableRouting()
        {

            // Register Dynamic MDM Routes
            Routing.RegisterRoute("DynamicRegistryPage", typeof(DynamicRegistryPage));
            Routing.RegisterRoute("DynamicEntryPage", typeof(DynamicEntryPage));

            // Keep your Voucher Routes
            Routing.RegisterRoute("VoucherEntryPage", typeof(Views.Accounting.VoucherEntryPage));
            Routing.RegisterRoute("VoucherRegistryPage", typeof(Views.Accounting.VoucherRegistryPage));

            // Register POS Routes
            Routing.RegisterRoute("PosPage", typeof(Views.Sales.PosPage));
            // Add more routes as needed
            // Register  Dashboard Routes
            Routing.RegisterRoute("DashboardPage", typeof(Views.DashboardPage));

        }
        public static MauiAppBuilder UseGarmetixBaseUI(this MauiAppBuilder builder)
        {
            //Seeders
            builder.Services.AddTransient<Garmetix.UI.Seeders.DatabaseSeeder>();


            // Register services, handlers, etc. here.
            // For example:
            // builder.Services.AddSingleton<IMyService, MyService>();
            // 1. Register the Dynamic Master Data Service (Crucial for MDM)
            builder.Services.AddSingleton<MasterDataService>();
            builder.Services.AddSingleton<INotificationService, AppNotificationService>();
            // 2. Register the Dynamic UI Pages & ViewModels
            builder.Services.AddTransient<DynamicRegistryViewModel>();
            builder.Services.AddTransient<DynamicRegistryPage>();
            builder.Services.AddTransient<DynamicEntryViewModel>();
            builder.Services.AddTransient<DynamicEntryPage>();

            builder.Services.AddTransient<Garmetix.UI.ViewModels.Sales.PosViewModel>();
            builder.Services.AddTransient<Garmetix.UI.Views.Sales.PosPage>();


            builder.Services.AddTransient<DashboardViewModel>();
            builder.Services.AddSingleton<DashboardPage>();

            // Voucher Page and ViewModel
            builder.Services.AddTransient<Garmetix.UI.ViewModels.Accounting.VoucherEntryViewModel>();
            builder.Services.AddTransient<Garmetix.UI.Views.Accounting.VoucherEntryPage>();
            builder.Services.AddTransient<Garmetix.UI.ViewModels.Accounting.VoucherRegistryViewModel>();
            builder.Services.AddTransient<Garmetix.UI.Views.Accounting.VoucherRegistryPage>();

            //Check any ViewModel and Page registrations you might be missing here, especially if you added new ones.
           
            return builder;
        }
    }
}
