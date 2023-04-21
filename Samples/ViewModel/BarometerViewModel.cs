using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class BarometerViewModel : BaseViewModel
    {
        private bool isActive;
        private double pressure;
        private int speed = 0;

        public BarometerViewModel()
        {
            this.StartCommand = new Command(this.OnStartBarometer);
            this.StopCommand = new Command(this.OnStop);
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public bool IsActive
        {
            get => this.isActive;
            set => this.SetProperty(ref this.isActive, value);
        }

        public double Pressure
        {
            get => this.pressure;
            set => this.SetProperty(ref this.pressure, value);
        }

        public string[] Speeds { get; } =
           Enum.GetNames(typeof(SensorSpeed));

        public int Speed
        {
            get => this.speed;
            set => this.SetProperty(ref this.speed, value);
        }

        public override void OnAppearing()
        {
            Barometer.ReadingChanged += this.OnBarometerReadingChanged;
            base.OnAppearing();
        }

        public override void OnDisappearing()
        {
            this.OnStop();
            Barometer.ReadingChanged -= this.OnBarometerReadingChanged;

            base.OnDisappearing();
        }

        private async void OnStartBarometer()
        {
            try
            {
                Barometer.Start((SensorSpeed)this.Speed);
                this.IsActive = true;
            }
            catch (Exception ex)
            {
                await this.DisplayAlertAsync($"Unable to start barometer: {ex.Message}");
            }
        }

        private void OnBarometerReadingChanged(object sender, BarometerChangedEventArgs e)
        {
            this.Pressure = e.Reading.PressureInHectopascals;
        }

        private void OnStop()
        {
            try
            {
                this.IsActive = false;
                Barometer.Stop();
                Barometer.ReadingChanged -= this.OnBarometerReadingChanged;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"An exception occured: {ex.Message}");
            }
        }
    }
}
