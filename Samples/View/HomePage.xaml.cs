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

            //Crashes.GenerateTestCrash();
        }

        async void OnSampleTapped(object sender, ItemTappedEventArgs e)
        {
            var item = e.Item as SampleItem;
            if (item == null)
            {
                return;
            }

            await this.Navigation.PushAsync((Page)Activator.CreateInstance(item.PageType));

            IDictionary<string, string> properties = new Dictionary<string, string> { { "PageType", $"{item.PageType.Name}" } };
            Analytics.TrackEvent("PushAsync", properties);

            // deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}