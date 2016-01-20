using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace YJMPD_UWP.ViewModels
{
    public class MatchVM : TemplateVM
    {
        public MatchVM() : base("Match")
        {
            Error = "";
            Message = "";
            App.Game.OnDestinationEnter += Game_OnDestinationEnter;
            App.Game.OnDestinationLeave += Game_OnDestinationLeave;
        }

        private void Game_OnDestinationLeave(object sender, System.EventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (App.Game.Selected)
                    Error = "Return to location!";

                NotifyPropertyChanged(nameof(ErrorVisible));
                NotifyPropertyChanged(nameof(Error));
            });
        }

        private void Game_OnDestinationEnter(object sender, System.EventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (App.Game.Selected)
                    Error = "";

                if (!App.Game.Selected)
                    Message = "You reached the destination!";

                NotifyPropertyChanged(nameof(ErrorVisible));
                NotifyPropertyChanged(nameof(Error));
                NotifyPropertyChanged(nameof(MessageVisible));
                NotifyPropertyChanged(nameof(Message));
            });   
        }

        public bool MessageVisible
        {
            get
            {
                return Message != "";
            }
        }

        public bool ErrorVisible
        {
            get
            {
                return Error != "";
            }
        }

        public string Message
        {
            get; private set;
        }

        public string Error
        {
            get; private set;
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
