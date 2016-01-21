using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.Foundation;
using Windows.Services.Maps;
using Windows.UI.Xaml.Media.Imaging;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Helpers.EventArgs;
using YJMPD_UWP.Model.Object;
using YJMPD_UWP.Views;

namespace YJMPD_UWP.Model
{
    public class GameHandler
    {
        public delegate void OnStatusUpdateHandler(object sender, GameStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

        public delegate void OnPlayersUpdateHandler(object sender, GamePlayersUpdatedEventArgs e);
        public event OnPlayersUpdateHandler OnPlayersUpdate;

        public delegate void OnDestinationEnteredHandler(object sender, EventArgs e);
        public event OnDestinationEnteredHandler OnDestinationEnter;

        public delegate void OnDestinationLeftHandler(object sender, EventArgs e);
        public event OnDestinationLeftHandler OnDestinationLeave;

        public enum GameStatus { STARTED, SEARCHING, WAITING, ENDED, STOPPED }
        public GameStatus Status { get; private set; }

        public List<Player> Players { get; private set; }
        public BasicGeoposition Destination { get; private set; }

        public bool Selected { get; private set; }

        private void UpdateGameStatus(GameStatus status)
        {
            Status = status;

            if (Status == GameStatus.STARTED)
                App.Geo.KeepHistory();
            else
                App.Geo.ClearHistory();

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new GameStatusUpdatedEventArgs(status));
        }
        private void UpdateGamePlayers(Player player)
        {
            if (OnPlayersUpdate == null) return;

            OnPlayersUpdate(this, new GamePlayersUpdatedEventArgs(player));
        }
        private void UpdateDestinationEnter()
        {
            if (OnDestinationEnter == null) return;

            OnDestinationEnter(this, new EventArgs());
        }
        private void UpdateDestinationLeave()
        {
            if (OnDestinationLeave == null) return;

            OnDestinationLeave(this, new EventArgs());
        }

        public GameHandler()
        {
            Players = new List<Player>();
            Status = GameStatus.STOPPED;
            Selected = false;
            Destination = new BasicGeoposition() { Altitude = -1 };
            App.Photo.OnStatusUpdate += Photo_OnStatusUpdate;
            GeofenceMonitor.Current.GeofenceStateChanged += Current_GeofenceStateChanged;
        }

        private void Current_GeofenceStateChanged(GeofenceMonitor sender, object args)
        {
            if (Status != GameStatus.STARTED) return;

            var reports = sender.ReadReports();

            foreach (GeofenceStateChangeReport report in reports)
            {
                GeofenceState state = report.NewState;
                Geofence geofence = report.Geofence;

                if (geofence.Id != "destination") continue;

                else if (state == GeofenceState.Entered)
                {
                    UpdateDestinationEnter();

                    if (!Selected)
                        App.Api.DestinationReached();
                }

                else if (state == GeofenceState.Exited)
                {
                    UpdateDestinationLeave();

                    if (Selected)
                        Util.ShowToastNotification("Left Area", "Please return to your location!");
                }
            }
        }

        private void Photo_OnStatusUpdate(object sender, PhotoStatusUpdatedEventArgs e)
        {
            switch(e.Status)
            {
                case PhotoHandler.PhotoStatus.UPLOADING:
                    App.Navigate(typeof(WaitingView), "Uploading...");
                    break;
                case PhotoHandler.PhotoStatus.DONE:
                    if(Selected)
                        App.Api.SendPhoto(App.Photo.Photo);
                    break;
            }
        }

        public void AddPlayer(string username)
        {
            Player p = new Player(username);
            Players.Add(p);
            UpdateGamePlayers(p);
        }

        public void SetSelected(bool b)
        {
            if (b)
            {
                Selected = true;
                Settings.Statistics.Selected += 1;
            }
            else
                Selected = false;
        }

        public void MoveToWaiting()
        {
            UpdateGameStatus(GameStatus.WAITING);
        }

        public void MoveToStarted(BasicGeoposition bgps)
        {
            Destination = bgps;

            App.Navigate(typeof(MatchView));

            GeofenceMonitor.Current.Geofences.Add(new Geofence("destination", new Geocircle(bgps, 50), MonitoredGeofenceStates.Entered | MonitoredGeofenceStates.Exited, false, TimeSpan.FromSeconds(1)));

            UpdateGameStatus(GameStatus.STARTED);
        }

        public void RemovePlayer(string username)
        {
            for(int i=Players.Count-1; i>=0; i--)
            {
                if (Players[i].Username == username)
                {
                    UpdateGamePlayers(Players[i]);
                    Players.RemoveAt(i);
                    return;
                }
            }         
        }

        public void Reset()
        {
            App.Photo.Reset();
            Selected = false;
            Destination = new BasicGeoposition() { Altitude = -1 };
            Players.Clear();
            GeofenceMonitor.Current.Geofences.Clear();
            UpdateGamePlayers(null);

            App.Navigate(typeof(GameView));
        }

        public void UpdatePlayer(string username, double pointstotal, double points)
        {
            foreach(Player p in Players)
            {
                if(p.Username == username)
                {
                    p.Update(pointstotal, points);
                    UpdateGamePlayers(p);
                    return;
                }
            }
        }

        public Player GetPlayer(string username)
        {
            foreach(Player p in Players)
            {
                if(p.Username == username)
                    return p;
            }

            return null;
        }

        //Ending

        public async Task<bool> StopMatch()
        {
            CalculateDistanceWalked();
            Settings.Statistics.Matches += 1;

            Settings.Statistics.AddPoints(GetPlayer(Settings.Username).Points);

            Settings.SaveStatistics();

            GeofenceMonitor.Current.Geofences.Clear();
            Selected = false;
            App.Photo.Reset();
            Destination = new BasicGeoposition() { Altitude = -1 };

            App.Navigate(typeof(ScoreView));
            UpdateGameStatus(GameStatus.ENDED);

            return true;
        }

        public async void CalculateDistanceWalked()
        {
            if (App.Geo.History.Count <= 1) return;

            MapRoute r = await Util.FindWalkingRoute(App.Geo.History.Select(p => p.Coordinate.Point).ToList());
            Settings.Statistics.Distance += r.LengthInMeters;
        }

        //Starting and Stopping

        public async Task<bool> StartGame()
        {
            UpdateGameStatus(GameStatus.SEARCHING);
            return await App.Api.JoinGame();
        }

        public async Task<bool> StopGame()
        {
            App.Api.LeaveGame();

            Reset();

            UpdateGameStatus(GameStatus.STOPPED);
            return true;
        }


        public void BackToGame()
        {
            switch(Status)
            {
                default:
                case GameStatus.STOPPED:
                    break;
                case GameStatus.SEARCHING:
                    App.Navigate(typeof(GameView));
                    break;
                case GameStatus.WAITING:
                    if (Selected)
                        App.Navigate(typeof(PhotoView));
                    else
                        App.Navigate(typeof(WaitingView));
                    break;
                case GameStatus.STARTED:
                    App.Navigate(typeof(MatchView));
                    break;
                case GameStatus.ENDED:
                    App.Navigate(typeof(ScoreView));
                    break;
            }
        }
    }
}
