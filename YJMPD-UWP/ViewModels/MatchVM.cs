using System.Collections.Generic;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.ViewModels
{
    public class MatchVM : TemplateVM
    {
        public MatchVM() : base("Match")
        {
            App.Geo.OnStatusUpdate += Geo_OnStatusUpdate;
            App.Network.OnStatusUpdate += Network_OnStatusUpdate;
            App.Game.OnStatusUpdate += Game_OnStatusUpdate;
            App.Game.OnPlayersUpdate += Game_OnPlayersUpdate;
        }

        private void Game_OnPlayersUpdate(object sender, Helpers.EventArgs.GamePlayersUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(Players));
                NotifyPropertyChanged(nameof(PlayersCount));
            });
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
                NotifyPropertyChanged(nameof(ConnectingServer));
            });
        }

        private void Geo_OnStatusUpdate(object sender, Helpers.EventArgs.PositionStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(MatchAvailable));
                NotifyPropertyChanged(nameof(ConnectingServer));
            });
        }
        

        public List<Player> Players
        {
            get
            {
                return new List<Player>(App.Game.Players);
            }
        }

        public string PlayersCount
        {
            get
            {
                if (App.Game.Players.Count == 1)
                    return "There is currently " + App.Game.Players.Count + " player in the match.";
                else
                    return "There are currently " + App.Game.Players.Count + " players in the match.";
            }
        }

        public bool MatchAvailable
        {
            get
            {
                return App.Geo.Status == Windows.Devices.Geolocation.PositionStatus.Ready && App.Network.Status == Model.NetworkHandler.NetworkStatus.CONNECTED;
            }
        }

        public bool ConnectingServer
        {
            get
            {
                return !MatchAvailable;
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
