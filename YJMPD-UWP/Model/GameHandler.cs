using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
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

        public bool Selected { get; set; }

        private void UpdateGameStatus(GameStatus status)
        {
            Status = status;

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

        public void RemovePlayer(string username)
        {
            Players.ForEach(p => {
                if(p.Username == username)
                {
                    Players.Remove(p);
                }
            });
        }

        public void Reset()
        {
            Players.Clear();
            UpdateGamePlayers(null);
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


        //Searching

        public async Task<bool> Search()
        {
            UpdateGameStatus(GameStatus.SEARCHING);
            return await App.Api.SearchGame();
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

            Search();

            return true;
        }

        private async Task<bool> StopGame()
        {
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
