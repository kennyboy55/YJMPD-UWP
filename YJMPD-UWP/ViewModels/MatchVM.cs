using System.Diagnostics;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using YJMPD_UWP.Helpers;

namespace YJMPD_UWP.ViewModels
{
    public class MatchVM : TemplateVM
    {
        private int angle;

        public MatchVM() : base("Match")
        {
            Error = "";
            Message = "";
            Degrees = 0;
            angle = (int)Util.DegreeBearing(App.Geo.Position.Coordinate.Point.Position, App.Game.Destination);
            HeadingVisible = false;
            App.Game.OnDestinationEnter += Game_OnDestinationEnter;
            App.Game.OnDestinationLeave += Game_OnDestinationLeave;
            App.Geo.OnPositionUpdate += Geo_OnPositionUpdate;
            App.Compass.OnHeadingUpdate += Compass_OnHeadingUpdate;
        }

        private void Compass_OnHeadingUpdate(object sender, Helpers.EventArgs.HeadingUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (App.Game.Status != Model.GameHandler.GameStatus.STARTED)
                    return;

                Degrees = (int)(angle + -e.Heading.HeadingMagneticNorth);
                NotifyPropertyChanged(nameof(Degrees));
            });
        }

        private void Geo_OnPositionUpdate(object sender, Helpers.EventArgs.PositionUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                if (App.Game.Status != Model.GameHandler.GameStatus.STARTED)
                    return;

                angle = (int)Util.DegreeBearing(e.Position.Coordinate.Point.Position, App.Game.Destination);
                HeadingVisible = Util.Distance(e.Position.Coordinate.Point.Position, App.Game.Destination) > 500;

                NotifyPropertyChanged(nameof(HeadingVisible));
                NotifyPropertyChanged(nameof(InvHeadingVisible));
            });
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

        public int Degrees
        {
            get; private set;
        }

        public bool HeadingVisible
        {
            get; private set;
        }

        public bool InvHeadingVisible
        {
            get
            {
                return !HeadingVisible;
            }
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
