using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class NetworkHandler
    {
        public delegate void OnStatusUpdatedHandler(object sender, NetworkStatusUpdatedEventArgs e);
        public OnStatusUpdatedHandler OnStatusUpdated;

        public enum NetworkStatus { DISCONNECTED, CONNECTING, CONNECTED }

        private void UpdateNetworkStatus(NetworkStatus status)
        {
            if (OnStatusUpdated == null) return;

            OnStatusUpdated(this, new NetworkStatusUpdatedEventArgs(status));
        }

        public NetworkHandler()
        {

        }
    }
}
