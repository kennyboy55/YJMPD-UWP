using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PhotoStatusUpdatedEventArgs : System.EventArgs
    {
        public Model.PhotoHandler.PhotoStatus Status { get; private set; }

        public PhotoStatusUpdatedEventArgs(Model.PhotoHandler.PhotoStatus status)
        {
            Status = status;
        }
    }
}
