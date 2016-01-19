using Windows.Devices.Geolocation;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PositionUpdatedEventArgs : System.EventArgs
    {
        public Geoposition Position { get; private set; }

        public PositionUpdatedEventArgs(Geoposition pos)
        {
            Position = pos;
        }
    }
}
