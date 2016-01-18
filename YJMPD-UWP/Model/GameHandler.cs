using System.Threading.Tasks;
using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class GameHandler
    {
        public delegate void OnStatusUpdateHandler(object sender, GameStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

        public enum GameStatus { STOPPING, STOPPED, SEARCHING, WAITING, ENDED, STARTING, STARTED }
        public GameStatus Status { get; private set; }

        private void UpdateGameStatus(GameStatus status)
        {
            Status = status;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new GameStatusUpdatedEventArgs(status));
        }

        public GameHandler()
        {
            Status = GameStatus.STOPPED;
        }




        //Searching

        public async Task<bool> Search()
        {
            return await SearchGame();
        }

        private async Task<bool> SearchGame()
        {
            UpdateGameStatus(GameStatus.SEARCHING);

            bool foundgame;

            foundgame = await App.Network.SearchGame();

            //Do stuff

            if (foundgame)
                await Start();
            else
                await Stop();
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
            UpdateGameStatus(GameStatus.STARTING);

            //Do stuff

            UpdateGameStatus(GameStatus.STARTED);
            return true;
        }

        private async Task<bool> StopGame()
        {
            UpdateGameStatus(GameStatus.STOPPING);

            //Do stuff

            UpdateGameStatus(GameStatus.STOPPED);
            return true;
        }
    }
}
