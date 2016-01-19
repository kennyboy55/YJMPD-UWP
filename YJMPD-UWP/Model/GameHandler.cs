using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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

        public enum GameStatus { STARTED, SEARCHING, WAITING, ENDED, STOPPED }
        public GameStatus Status { get; private set; }

        public List<Player> Players { get; private set; }

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

        public GameHandler()
        {
            Players = new List<Player>();
            Status = GameStatus.STOPPED;
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
            Players.Clear();
            GeofenceMonitor.Current.Geofences.Clear();
            UpdateGamePlayers(null);

            App.Navigate(typeof(MatchView));
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

        //Ending

        public async Task<bool> End()
        {
            CalculateDistanceWalked();
            Settings.Statistics.Matches += 1;

            UpdateGameStatus(GameStatus.ENDED);

            return true;
        }

        public async void CalculateDistanceWalked()
        {
            MapRoute r = await Util.FindWalkingRoute(App.Geo.History.Select(p => p.Coordinate.Point).ToList());
            Settings.Statistics.Distance += r.LengthInMeters;
        }

        //Starting and Stopping

        public async Task<bool> Start()
        {
            return await StartGame();
        }

        public async Task<bool> Stop()
        {
            return await StopGame();
        }

        private async Task<bool> StartGame()
        {
            UpdateGameStatus(GameStatus.SEARCHING);
            return await App.Api.JoinGame();
        }

        private async Task<bool> StopGame()
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
                    App.Navigate(typeof(MatchView));
                    break;
                case GameStatus.WAITING:
                    if (Selected)
                        App.Navigate(typeof(PhotoView));
                    else
                        App.Navigate(typeof(WaitingView));
                    break;
                case GameStatus.STARTED:
                    if (Selected && App.Photo.Photo == null)
                        App.Navigate(typeof(PhotoView));
                    else
                        App.Navigate(typeof(GameView));
                    break;
                case GameStatus.ENDED:
                    App.Navigate(typeof(ScoreView));
                    break;
            }
        }
    }
}
