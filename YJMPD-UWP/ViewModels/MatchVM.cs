namespace YJMPD_UWP.ViewModels
{
    public class MatchVM : TemplateVM
    {
        public MatchVM() : base("Match")
        {
            App.Geo.OnStatusUpdate += Geo_OnStatusUpdate;
            App.Network.OnStatusUpdate += Network_OnStatusUpdate;
            App.Game.OnStatusUpdate += Game_OnStatusUpdate;
        }

        private void Game_OnStatusUpdate(object sender, Helpers.EventArgs.GameStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(StartMatch));
                NotifyPropertyChanged(nameof(StopMatch));
            });
        }

        private void Network_OnStatusUpdate(object sender, Helpers.EventArgs.NetworkStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(MatchAvailable));
            });
        }

        private void Geo_OnStatusUpdate(object sender, Helpers.EventArgs.PositionStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(MatchAvailable));
            });
        }
        

        public bool MatchAvailable
        {
            get
            {
                return App.Geo.Status == Windows.Devices.Geolocation.PositionStatus.Ready && App.Network.Status == Model.NetworkHandler.NetworkStatus.CONNECTED;
            }
        }

        public bool StartMatch
        {
            get
            {
                return App.Game.Status == Model.GameHandler.GameStatus.STOPPED;
            }
        }

        public bool StopMatch
        {
            get
            {
                return !StartMatch;
            }
        }
    }
}
