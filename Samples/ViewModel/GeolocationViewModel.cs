using System;
using System.Threading;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class GeolocationViewModel : BaseViewModel
    {
        private readonly string notAvailable = "not available";
        private string lastLocation;
        private string currentLocation;
        private int accuracy = (int)GeolocationAccuracy.Default;
        private CancellationTokenSource cts;

        public GeolocationViewModel()
        {
            this.GetLastLocationCommand = new Command(this.OnGetLastLocation);
            this.GetCurrentLocationCommand = new Command(this.OnGetCurrentLocation);
        }

        public ICommand GetLastLocationCommand { get; }

        public ICommand GetCurrentLocationCommand { get; }

        public string LastLocation
        {
            get => this.lastLocation;
            set => this.SetProperty(ref this.lastLocation, value);
        }

        public string CurrentLocation
        {
            get => this.currentLocation;
            set => this.SetProperty(ref this.currentLocation, value);
        }

        public string[] Accuracies
            => Enum.GetNames(typeof(GeolocationAccuracy));

        public int Accuracy
        {
            get => this.accuracy;
            set => this.SetProperty(ref this.accuracy, value);
        }

        private async void OnGetLastLocation()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();
                this.LastLocation = this.FormatLocation(location);
            }
            catch (Exception ex)
            {
                this.LastLocation = this.FormatLocation(null, ex);
            }
            this.IsBusy = false;
        }

        private async void OnGetCurrentLocation()
        {
            if (this.IsBusy)
            {
                return;
            }

            this.IsBusy = true;
            try
            {
                var request = new GeolocationRequest((GeolocationAccuracy)this.Accuracy);
                this.cts = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, this.cts.Token);
                this.CurrentLocation = this.FormatLocation(location);
            }
            catch (Exception ex)
            {
                this.CurrentLocation = this.FormatLocation(null, ex);
            }
            finally
            {
                this.cts.Dispose();
                this.cts = null;
            }
            this.IsBusy = false;
        }

        private string FormatLocation(Location location, Exception ex = null)
        {
            return location == null
                ? $"Unable to detect location. Exception: {ex?.Message ?? string.Empty}"
                : $"Latitude: {location.Latitude}\n" +
                $"Longitude: {location.Longitude}\n" +
                $"HorizontalAccuracy: {location.Accuracy}\n" +
                $"Altitude: {(location.Altitude.HasValue ? location.Altitude.Value.ToString() : this.notAvailable)}\n" +
                $"AltitudeRefSys: {location.AltitudeReferenceSystem}\n" +
                $"VerticalAccuracy: {(location.VerticalAccuracy.HasValue ? location.VerticalAccuracy.Value.ToString() : this.notAvailable)}\n" +
                $"Heading: {(location.Course.HasValue ? location.Course.Value.ToString() : this.notAvailable)}\n" +
                $"Speed: {(location.Speed.HasValue ? location.Speed.Value.ToString() : this.notAvailable)}\n" +
                $"Date (UTC): {location.Timestamp:d}\n" +
                $"Time (UTC): {location.Timestamp:T}\n" +
                $"Moking Provider: {location.IsFromMockProvider}";
        }

        public override void OnDisappearing()
        {
            if (this.IsBusy)
            {
                if (this.cts != null && !this.cts.IsCancellationRequested)
                {
                    this.cts.Cancel();
                }
            }
            base.OnDisappearing();
        }
    }
}
