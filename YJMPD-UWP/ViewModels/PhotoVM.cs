using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.ViewModels
{
    public class PhotoVM : TemplateVM
    {
        private string photo;

        DispatcherTimer timer;
        int secondsleft = 60;

        public PhotoVM() : base("Photo")
        {
            App.Photo.OnPhotoTaken += Photo_OnPhotoTaken;
            photo = App.Photo.Photo;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;

            if (ControlsVisible)
                timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            NotifyPropertyChanged(nameof(TimeOut));

            secondsleft--;
            if(secondsleft <= 0)
            {
                App.Game.Stop();   
            }
        }

        private void Photo_OnPhotoTaken(object sender, Helpers.EventArgs.PhotoTakenEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                timer.Stop();
                secondsleft = 60;

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
                return "Take a photo within " + secondsleft + " seconds";
            }
        }

        public ImageSource Photo
        {
            get
            {
                if (photo == null || photo == "")
                    return new BitmapImage(new Uri("ms-appx:///Assets/Error.png"));

                return new BitmapImage(new Uri(photo));
            }
        }
    }
}
