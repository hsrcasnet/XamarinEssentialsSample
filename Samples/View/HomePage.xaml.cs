using System;
using System.Collections.Generic;
using Microsoft.AppCenter.Analytics;
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

            // DEMO: Use TrackEvent to write diagnostics info to App Center
            var properties = new Dictionary<string, string>
            {
                { "PageName", $"{item.PageType.Name}"}
            };
            Analytics.TrackEvent("PageView", properties);
            Analytics.TrackEvent($"PageView: {item.PageType.Name}");

            await this.Navigation.PushAsync((Page)Activator.CreateInstance(item.PageType));

            // deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
