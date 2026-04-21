using Garmetix.UI.Seeders;
using Microsoft.Extensions.DependencyInjection;

namespace Garmetix.ERP
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            // Load the AppShell, but it will be hidden initially by the Auth router
            MainPage = new AppShell();
            Garmetix.UI.GarmetixUIModule.EnableRouting();
        }
        protected override async void OnStart()
        {
            base.OnStart();
            // 1. RUN THE SEEDER
            // We use Handler.MauiContext to grab the DI container during startup
            var seeder = Handler.MauiContext.Services.GetRequiredService<DatabaseSeeder>();
            await seeder.SeedAsync();

            // 2. AUTHENTICATION ROUTING
            // Check the secure keychain to see if the user previously logged in
            string hasPin = await SecureStorage.Default.GetAsync("HasPin");

            if (hasPin == "true")
            {
                // Bypass full login, go straight to the fast PIN Unlock screen
                await Shell.Current.GoToAsync("//PinUnlockPage");
            }
            else
            {
                // First time opening the app, or they explicitly logged out
                await Shell.Current.GoToAsync("//LoginPage");
            }
        }
        //protected override Window CreateWindow(IActivationState? activationState)
        //{
        //    return new Window(new AppShell());
        //}
    }
}