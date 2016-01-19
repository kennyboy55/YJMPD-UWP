using Windows.UI.Xaml.Media;

namespace YJMPD_UWP.ViewModels
{
    public class PhotoVM : TemplateVM
    {
        private ImageSource photo;

        public PhotoVM() : base("Photo")
        {
            App.Photo.OnPhotoTaken += Photo_OnPhotoTaken;
        }

        private void Photo_OnPhotoTaken(object sender, Helpers.EventArgs.PhotoTakenEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                photo = e.Photo;
                NotifyPropertyChanged(nameof(Photo));
                NotifyPropertyChanged(nameof(PhotoVisible));
                NotifyPropertyChanged(nameof(ControlsVisible));
            });
        }

        public bool PhotoVisible
        {
            get
            {
                return photo != null;
            }
        }

        public bool ControlsVisible
        {
            get
            {
                return !PhotoVisible;
            }
        }

        public string TimeOut
        {
            get
            {
                return "Take a photo within 1 minute";
            }
        }

        public ImageSource Photo
        {
            get
            {
                return photo;
            }
        }
    }
}
