using Windows.Devices.Geolocation;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class MatchView : Page
    {
        MatchVM matchvm;
        MapIcon pos;
        Geoposition old;

        public MatchView()
        {
            matchvm = new MatchVM();
            this.DataContext = matchvm;
            this.InitializeComponent();

            pos = new MapIcon();
            pos.Title = "You";
            pos.ZIndex = 100;

            App.Geo.OnPositionUpdate += Geo_OnPositionUpdate;
        }

        private void Geo_OnPositionUpdate(object sender, Helpers.EventArgs.PositionUpdatedEventArgs e)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, () =>
            {
                pos.Location = e.Position.Coordinate.Point;

                if (!Map.MapElements.Contains(pos))
                    Map.MapElements.Add(pos);


                if(old != null && !App.Game.Selected)
                {
                    Map.MapElements.Add(Util.GetRouteLine(old.Coordinate.Point.Position, e.Position.Coordinate.Point.Position, Color.FromArgb(255, 200, 50, 50), 50));
                }

                old = e.Position;

                Map.TrySetViewAsync(e.Position.Coordinate.Point);
                Map.TryZoomToAsync(13);
            });
        }
    }
}
