using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.ViewModels
{
    public class GameVM : TemplateVM
    {
        public GameVM() : base("Game")
        {

        }

        public ImageSource Photo
        {
            get
            {
                return new BitmapImage(new System.Uri(App.Photo.Photo));
            }
        }
    }
}
