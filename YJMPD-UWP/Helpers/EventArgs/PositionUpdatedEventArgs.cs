using Windows.Devices.Geolocation;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PositionUpdatedEventArgs : System.EventArgs
    {
        public Geoposition Old { get; private set; }
        public Geoposition New { get; private set; }

        public PositionUpdatedEventArgs(Geoposition old, Geoposition notold)
        {
            Old = old;
            New = notold;
        }
    }
}
