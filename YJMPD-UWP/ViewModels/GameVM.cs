using System.Collections.Generic;
using System.Diagnostics;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.ViewModels
{
    public class GameVM : TemplateVM
    {
        public GameVM() : base("Game")
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
                NotifyPropertyChanged(nameof(StartGame));
                NotifyPropertyChanged(nameof(StopGame));
            });
        }

        private void Network_OnStatusUpdate(object sender, Helpers.EventArgs.NetworkStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(GameAvailable));
                NotifyPropertyChanged(nameof(ServerAvailable));
                NotifyPropertyChanged(nameof(ServerMessage));
            });
        }

        private void Geo_OnStatusUpdate(object sender, Helpers.EventArgs.PositionStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(GameAvailable));
                NotifyPropertyChanged(nameof(ServerAvailable));
                NotifyPropertyChanged(nameof(ServerMessage));
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
                    return "There is currently " + App.Game.Players.Count + " player in the game.";
                else
                    return "There are currently " + App.Game.Players.Count + " players in the game.";
            }
        }

        public bool GameAvailable
        {
            get
            {
                return App.Geo.Status == Windows.Devices.Geolocation.PositionStatus.Ready && App.Network.Status == Model.NetworkHandler.NetworkStatus.CONNECTED;
            }
        }

        public bool ServerAvailable
        {
            get
            {
                return !GameAvailable;
            }
        }

        public string ServerMessage
        {
            get
            {
                string str = "";

                switch (App.Network.Status)
                {
                    case Model.NetworkHandler.NetworkStatus.DISCONNECTED:
                        str = "Disconnected";
                        break;
                    case Model.NetworkHandler.NetworkStatus.CONNECTING:
                        str = "Connecting to server...";
                        break;
                }

                switch(App.Geo.Status)
                {
                    case Windows.Devices.Geolocation.PositionStatus.Disabled:
                    case Windows.Devices.Geolocation.PositionStatus.NotAvailable:
                    case Windows.Devices.Geolocation.PositionStatus.NoData:
                        str = "GPS not available";
                        break;
                    case Windows.Devices.Geolocation.PositionStatus.NotInitialized:
                    case Windows.Devices.Geolocation.PositionStatus.Initializing:
                        str = "Waiting on GPS...";
                        break;
                }

                if (str == "")
                    str = "Connected";

                return str;
            }
        }

        public bool StartGame
        {
            get
            {
                return App.Game.Status == Model.GameHandler.GameStatus.STOPPED;
            }
        }

        public bool StopGame
        {
            get
            {
                return !StartGame;
            }
        }
    }
}
