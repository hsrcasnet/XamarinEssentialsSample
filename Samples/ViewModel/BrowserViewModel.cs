using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class BrowserViewModel : BaseViewModel
    {
        private string browserStatus;
        private string uri = "http://xamarin.com";
        private int browserType = (int)BrowserLaunchMode.SystemPreferred;
        private int browserTitleType = (int)BrowserTitleMode.Default;
        private int controlColor = 0;
        private int toolbarColor = 0;
        private bool presentAsFormSheet = false;
        private bool presentAsPageSheet = false;
        private bool launchAdjacent = false;
        private readonly Dictionary<string, Color> colorDictionary;

        public List<string> AllColors { get; }

        public BrowserViewModel()
        {
            this.OpenUriCommand = new Command(this.OpenUri);

            this.colorDictionary = typeof(Color)
                .GetFields()
                .Where(f => f.FieldType == typeof(Color) && f.IsStatic && f.IsPublic)
                .ToDictionary(f => f.Name, f => (Color)f.GetValue(null));

            var colors = this.colorDictionary.Keys.ToList();
            colors.Insert(0, "None");

            this.AllColors = colors;
        }

        public ICommand OpenUriCommand { get; }

        public string BrowserStatus
        {
            get => this.browserStatus;
            set => this.SetProperty(ref this.browserStatus, value);
        }

        public string Uri
        {
            get => this.uri;
            set => this.SetProperty(ref this.uri, value);
        }

        public List<string> BrowserLaunchModes { get; } =
            new List<string>
            {
                $"Use System-Preferred Browser",
                $"Use Default Browser App"
            };

        public int BrowserType
        {
            get => this.browserType;
            set => this.SetProperty(ref this.browserType, value);
        }

        public List<string> BrowserTitleModes { get; } =
            new List<string>
            {
                $"Use Default Mode",
                $"Show Title",
                $"Hide Title"
            };

        public int BrowserTitleType
        {
            get => this.browserTitleType;
            set => this.SetProperty(ref this.browserTitleType, value);
        }

        public int ToolbarColor
        {
            get => this.toolbarColor;
            set => this.SetProperty(ref this.toolbarColor, value);
        }

        public int ControlColor
        {
            get => this.controlColor;
            set => this.SetProperty(ref this.controlColor, value);
        }

        public bool PresentAsFormSheet
        {
            get => this.presentAsFormSheet;
            set => this.SetProperty(ref this.presentAsFormSheet, value);
        }

        public bool PresentAsPageSheet
        {
            get => this.presentAsPageSheet;
            set => this.SetProperty(ref this.presentAsPageSheet, value);
        }

        public bool LaunchAdjacent
        {
            get => this.launchAdjacent;
            set => this.SetProperty(ref this.launchAdjacent, value);
        }

        private async void OpenUri()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            try
            {
                var flags = BrowserLaunchFlags.None;
                if (this.PresentAsPageSheet)
                {
                    flags |= BrowserLaunchFlags.PresentAsPageSheet;
                }

                if (this.PresentAsFormSheet)
                {
                    flags |= BrowserLaunchFlags.PresentAsFormSheet;
                }

                if (this.LaunchAdjacent)
                {
                    flags |= BrowserLaunchFlags.LaunchAdjacent;
                }

                await Browser.OpenAsync(this.uri, new BrowserLaunchOptions
                {
                    LaunchMode = (BrowserLaunchMode)this.BrowserType,
                    TitleMode = (BrowserTitleMode)this.BrowserTitleType,
                    PreferredToolbarColor = GetColor(this.ToolbarColor),
                    PreferredControlColor = GetColor(this.ControlColor),
                    Flags = flags
                });
            }
            catch (Exception e)
            {
                this.BrowserStatus = $"Unable to open Uri {e.Message}";
                Debug.WriteLine(this.browserStatus);
            }
            finally
            {
                this.IsBusy = false;
            }

            Color? GetColor(int index)
            {
                return index <= 0
                    ? null
                    : (System.Drawing.Color)this.colorDictionary[this.AllColors[index]];
            }
        }
    }
}
