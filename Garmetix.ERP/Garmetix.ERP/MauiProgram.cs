using Garmetix.Authentication;
using Microsoft.Extensions.Logging;
using Garmetix.UI;
using Garmetix.Data;
using Syncfusion.Maui.Toolkit.Hosting;
using Syncfusion.Maui.Core.Hosting;

namespace Garmetix.ERP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseGarmetixDataModules()
                .UseGarmetixAuthentication()
                .UseGarmetixBaseUI()
               .ConfigureSyncfusionToolkit()
               .ConfigureSyncfusionCore()
                .UseMauiApp<App>()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ... setup dependencies ...

            // GLOBAL EXCEPTION HANDLING
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                var ex = args.ExceptionObject as Exception;
                // Ideally, log this to a file or Sentry/AppCenter. 
                System.Diagnostics.Debug.WriteLine($"CRITICAL UNHANDLED: {ex?.Message}");
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                // Prevents background task crashes from killing the app
                args.SetObserved();
                System.Diagnostics.Debug.WriteLine($"BACKGROUND CRASH: {args.Exception.Message}");
            };
             
            return builder.Build();
        }
    }
}
