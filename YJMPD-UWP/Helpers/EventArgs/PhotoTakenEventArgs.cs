using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PhotoTakenEventArgs : System.EventArgs
    {
        public SoftwareBitmapSource Photo { get; private set; }

        public PhotoTakenEventArgs(SoftwareBitmapSource photo)
        {
            Photo = photo;
        }
    }
}
