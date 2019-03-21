using Foundation;
using Microsoft.AppCenter.Distribute;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

namespace Samples.iOS
{
    [Register(nameof(AppDelegate))]
    public partial class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            Distribute.DontCheckForUpdatesInDebug();
            this.LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}