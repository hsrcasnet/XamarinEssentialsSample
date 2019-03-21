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
        public App()
        {
            this.InitializeComponent();

            // Enable currently experimental features
            ExperimentalFeatures.Enable(
                ExperimentalFeatures.EmailAttachments,
                ExperimentalFeatures.ShareFileRequest);

            VersionTracking.Track();

            this.MainPage = new NavigationPage(new HomePage());
        }

        protected override void OnStart()
        {
            if ((Device.RuntimePlatform == Device.Android && CommonConstants.AppCenterAndroid != "AC_ANDROID") ||
                (Device.RuntimePlatform == Device.iOS && CommonConstants.AppCenteriOS != "AC_IOS"))
            {
                AppCenter.LogLevel = LogLevel.Verbose;
                Crashes.ShouldProcessErrorReport = ShouldProcess;
                Crashes.ShouldAwaitUserConfirmation = ConfirmationHandler;

                AppCenter.Start(
                    $"ios={CommonConstants.AppCenteriOS};" +
                    $"android={CommonConstants.AppCenterAndroid}",
                    typeof(Analytics),
                    typeof(Crashes),
                    typeof(Distribute));
            }
        }

        private static bool ConfirmationHandler()
        {
            Crashes.NotifyUserConfirmation(UserConfirmation.AlwaysSend);

            return true;
        }

        private static bool ShouldProcess(ErrorReport report)
        {
            return true;
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