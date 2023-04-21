using Xamarin.Essentials;

namespace Samples.ViewModel
{
    public class BatteryViewModel : BaseViewModel
    {
        public BatteryViewModel()
        {
        }

        public double Level => Battery.ChargeLevel;

        public BatteryState State => Battery.State;

        public BatteryPowerSource PowerSource => Battery.PowerSource;

        public EnergySaverStatus EnergySaverStatus => Battery.EnergySaverStatus;

        public override void OnAppearing()
        {
            base.OnAppearing();

            Battery.BatteryInfoChanged += this.OnBatteryInfoChanged;
            Battery.EnergySaverStatusChanged += this.OnEnergySaverStatusChanged;
        }

        public override void OnDisappearing()
        {
            Battery.BatteryInfoChanged -= this.OnBatteryInfoChanged;
            Battery.EnergySaverStatusChanged -= this.OnEnergySaverStatusChanged;

            base.OnDisappearing();
        }

        private void OnEnergySaverStatusChanged(object sender, EnergySaverStatusChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.EnergySaverStatus));
        }

        private void OnBatteryInfoChanged(object sender, BatteryInfoChangedEventArgs e)
        {
            this.OnPropertyChanged(nameof(this.Level));
            this.OnPropertyChanged(nameof(this.State));
            this.OnPropertyChanged(nameof(this.PowerSource));
        }
    }
}
