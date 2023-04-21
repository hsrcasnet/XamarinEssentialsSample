using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Samples.ViewModel
{
    public class AccelerometerViewModel : BaseViewModel
    {
        private double x;
        private double y;
        private double z;
        private string shakeTime = string.Empty;
        private bool isActive;
        private int speed = 0;

        public AccelerometerViewModel()
        {
            this.StartCommand = new Command(this.OnStart);
            this.StopCommand = new Command(this.OnStop);
        }

        public ICommand StartCommand { get; }

        public ICommand StopCommand { get; }

        public string ShakeTime
        {
            get => this.shakeTime;
            set => this.SetProperty(ref this.shakeTime, value);
        }

        public double X
        {
            get => this.x;
            set => this.SetProperty(ref this.x, value);
        }

        public double Y
        {
            get => this.y;
            set => this.SetProperty(ref this.y, value);
        }

        public double Z
        {
            get => this.z;
            set => this.SetProperty(ref this.z, value);
        }

        public bool IsActive
        {
            get => this.isActive;
            set => this.SetProperty(ref this.isActive, value);
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
            Accelerometer.ReadingChanged += this.OnReadingChanged;
            Accelerometer.ShakeDetected += this.Accelerometer_OnShaked;

            base.OnAppearing();
        }

        private void Accelerometer_OnShaked(object sender, EventArgs e) =>
            this.ShakeTime = $"Shake detected: {DateTime.Now.ToLongTimeString()}";

        public override void OnDisappearing()
        {
            this.OnStop();
            Accelerometer.ReadingChanged -= this.OnReadingChanged;
            Accelerometer.ShakeDetected -= this.Accelerometer_OnShaked;
            base.OnDisappearing();
        }

        private async void OnStart()
        {
            try
            {
                Accelerometer.Start((SensorSpeed)this.Speed);
                this.IsActive = true;
            }
            catch (Exception ex)
            {
                await this.DisplayAlertAsync($"Unable to start accelerometer: {ex.Message}");
            }
        }

        private void OnStop()
        {
            this.IsActive = false;
            Accelerometer.Stop();
        }

        private void OnReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var data = e.Reading;
            switch ((SensorSpeed)this.Speed)
            {
                case SensorSpeed.Fastest:
                case SensorSpeed.Game:
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        this.X = data.Acceleration.X;
                        this.Y = data.Acceleration.Y;
                        this.Z = data.Acceleration.Z;
                    });
                    break;
                default:
                    this.X = data.Acceleration.X;
                    this.Y = data.Acceleration.Y;
                    this.Z = data.Acceleration.Z;
                    break;
            }
        }
    }
}
