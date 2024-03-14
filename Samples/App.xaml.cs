using System.Diagnostics;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using Samples.Helpers;
using Samples.View;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Device = Xamarin.Forms.Device;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace Samples
{
    public partial class App : Application
    {
        public static IVisual PreferredVisual { get; set; } = VisualMarker.Material;

        public App()
        {
            this.InitializeComponent();

            // Enable currently experimental features
            Device.SetFlags(new string[] { "MediaElement_Experimental" });

            VersionTracking.Track();

            this.MainPage = new NavigationPage(new HomePage());

            try
            {
                AppActions.OnAppAction += this.AppActions_OnAppAction;
            }
            catch (FeatureNotSupportedException ex)
            {
                Debug.WriteLine($"{nameof(AppActions)} Exception: {ex}");
            }
        }

        protected override async void OnStart()
        {
            AppCenter.Start(
                $"ios=ad52a6d1-2e81-4af1-9398-54e53603311e;" +
                $"android=c45fb280-e432-44cb-8b1c-eefa1c63fbd4;",
                typeof(Analytics),
                typeof(Crashes),
                typeof(Distribute));

            try
            {
                await AppActions.SetAsync(
                    new AppAction("app_info", "App Info", icon: "app_info_action_icon"),
                    new AppAction("battery_info", "Battery Info"));
            }
            catch (FeatureNotSupportedException ex)
            {
                Debug.WriteLine($"{nameof(AppActions)} Exception: {ex}");
            }
        }

        private void AppActions_OnAppAction(object sender, AppActionEventArgs e)
        {
            // Don't handle events fired for old application instances
            // and cleanup the old instance's event handler

            if (Application.Current != this && Application.Current is App app)
            {
                AppActions.OnAppAction -= app.AppActions_OnAppAction;
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                var page = e.AppAction.Id switch
                {
                    "battery_info" => new BatteryPage(),
                    "app_info" => new AppInfoPage(),
                    _ => default(Page)
                };

                if (page != null)
                {
                    await Application.Current.MainPage.Navigation.PopToRootAsync();
                    await Application.Current.MainPage.Navigation.PushAsync(page);
                }
            });
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
