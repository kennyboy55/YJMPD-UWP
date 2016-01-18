using System.Collections.Generic;
using System.Threading.Tasks;
using YJMPD_UWP.Helpers.EventArgs;

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

        public Dictionary<string, double> Players { get; private set; }

        private void UpdateGameStatus(GameStatus status)
        {
            Status = status;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new GameStatusUpdatedEventArgs(status));
        }
        private void UpdateGamePlayers(string username, double points)
        {
            if (OnPlayersUpdate == null) return;

            OnPlayersUpdate(this, new GamePlayersUpdatedEventArgs(username, points));
        }

        public GameHandler()
        {
            Players = new Dictionary<string, double>();
            Status = GameStatus.STOPPED;
        }

        public void AddPlayer(string username, double points)
        {
            Players.Add(username, points);
            UpdateGamePlayers(username, points);
        }
        public void UpdatePlayer(string username, double points)
        {
            Players.Remove(username);
            AddPlayer(username, points);
        }


        //Searching

        public async Task<bool> Search()
        {
            return await SearchGame();
        }

        private async Task<bool> SearchGame()
        {
            UpdateGameStatus(GameStatus.SEARCHING);

            await App.Network.SearchGame();

            return true;
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

            //Do stuff

            UpdateGameStatus(GameStatus.STARTED);
            return true;
        }

        private async Task<bool> StopGame()
        {

            //Do stuff

            UpdateGameStatus(GameStatus.STOPPED);
            return true;
        }
    }
}
