using YJMPD_UWP.Model;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class NetworkStatusUpdatedEventArgs : System.EventArgs
    {
        public NetworkHandler.NetworkStatus NetworkStatus { get; private set; }

        public NetworkStatusUpdatedEventArgs(NetworkHandler.NetworkStatus status)
        {
            this.NetworkStatus = status;
        }
    }
}
