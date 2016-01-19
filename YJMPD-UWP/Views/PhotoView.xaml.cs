using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class PhotoView : Page
    {
        PhotoVM photovm;

        public PhotoView()
        {
            photovm = new PhotoVM();
            this.DataContext = photovm;
            this.InitializeComponent();
        }

        private void PhotoButton_Click(object sender, RoutedEventArgs e)
        {
            App.Photo.Take();
        }
    }
}
