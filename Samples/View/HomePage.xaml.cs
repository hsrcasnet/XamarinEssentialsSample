using System;
using System.Collections.Generic;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Samples.Model;
using Xamarin.Forms;

namespace Samples.View
{
    public partial class HomePage : BasePage
    {
        public HomePage()
        {
            this.InitializeComponent();
        }

        private async void OnSampleTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is not SampleItem item)
            {
                return;
            }

            // Demo: Use TrackEvent to write diagnostics info to AppCenter
            var properties = new Dictionary<string, string>
            {
                { "PageName", $"{item.PageType.Name}"}
            };
            Analytics.TrackEvent("PageView", properties);
            Analytics.TrackEvent($"PageView: {item.PageType.Name}");

            try
            {
                await this.Navigation.PushAsync((Page)Activator.CreateInstance(item.PageType));

                // Demo: Throw exception if BatteryPage is opened
                if (item.PageType == typeof(BatteryPage))
                {
                    throw new InvalidOperationException("This is just a test exception", new NullReferenceException("Something cannot be null"));
                }
            }
            catch (Exception ex)
            {
                // Demo: Use TrackError to write error/crash information to AppCenter
                Crashes.TrackError(ex);
            }

            // Demo: App crashes are automatically reported to AppCenter
            //throw new Exception("Test");

            // deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
