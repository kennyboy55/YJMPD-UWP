using YJMPD_UWP.Model;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class GameStatusUpdatedEventArgs : System.EventArgs
    {
        public GameHandler.GameStatus GameStatus { get; private set; }

        public GameStatusUpdatedEventArgs(GameHandler.GameStatus status)
        {
            this.GameStatus = status;
        }
    }
}
