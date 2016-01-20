using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class PhotoTakenEventArgs : System.EventArgs
    {
        public string Photo { get; private set; }

        public PhotoTakenEventArgs(string photo)
        {
            Photo = photo;
        }
    }
}
