using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class GameHandler
    {
        public delegate void OnStatusUpdateHandler(object sender, GameStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

        public enum GameStatus { STOPPED, SEARCHING, WAITING, ENDED, STARTING, STARTED }

        private void UpdateGameStatus(GameStatus status)
        {
            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new GameStatusUpdatedEventArgs(status));
        }
    }
}
