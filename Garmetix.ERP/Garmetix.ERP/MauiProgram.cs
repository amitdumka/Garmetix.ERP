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

            //builder.Services.AddGarmetixAuthentication();
            //builder.UseGarmetixBaseUI();
            return builder.Build();
        }
    }
}
