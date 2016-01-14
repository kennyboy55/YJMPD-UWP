using Windows.Devices.Geolocation;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PositionStatusUpdatedEventArgs : System.EventArgs
    {
        public PositionStatus Status { get; private set; }

        public PositionStatusUpdatedEventArgs(PositionStatus status)
        {
            Status = status;
        }
    }
}
