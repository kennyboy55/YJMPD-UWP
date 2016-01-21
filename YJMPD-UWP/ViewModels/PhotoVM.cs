using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.ViewModels
{
    public class PhotoVM : TemplateVM
    {

        DispatcherTimer timer;
        int secondsleft = 60;

        public PhotoVM() : base("Photo")
        {

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

            App.Photo.OnStatusUpdate += Photo_OnStatusUpdate;
        }

        private void Photo_OnStatusUpdate(object sender, Helpers.EventArgs.PhotoStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (e.Status == Model.PhotoHandler.PhotoStatus.NOPHOTO)
                    timer.Start();
                else
                {
                    secondsleft = 60;
                    timer.Stop();
                }
            });
        }

        private void Timer_Tick(object sender, object e)
        {
            NotifyPropertyChanged(nameof(TimeOut));

            secondsleft--;
            if(secondsleft <= 0)
            {
                timer.Stop();
                secondsleft = 60;
                App.Game.StopGame();   
            }
        }

        public string TimeOut
        {
            get
            {
                return "Take a photo within " + secondsleft + " seconds";
            }
        }
    }
}
