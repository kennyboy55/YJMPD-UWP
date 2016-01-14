using Windows.Devices.Sensors;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class HeadingUpdatedEventArgs : System.EventArgs
    {
        public CompassReading Heading;

        public HeadingUpdatedEventArgs(CompassReading heading)
        {
            Heading = heading;
        }
    }
}
