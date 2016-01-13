using System;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using YJMPD_UWP;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.ViewModels
{
    public class MainPageVM : TemplateVM
    {
        public MainPageVM() : base(Util.Loader.GetString("Loading"))
        {
            App.Geo.OnPositionUpdate += Geo_OnPositionUpdate;
            App.Geo.OnStatusUpdate += Geo_OnStatusUpdate;
        }

        protected override void UpdatePropertiesToNewLanguage()
        {
            NotifyPropertyChanged(nameof(Map));
            NotifyPropertyChanged(nameof(Help));
            NotifyPropertyChanged(nameof(Route));
            NotifyPropertyChanged(nameof(Landmarks));
            NotifyPropertyChanged(nameof(Search));
            NotifyPropertyChanged(nameof(Settings));
            NotifyPropertyChanged(nameof(Status));
            NotifyPropertyChanged(nameof(Source));
            NotifyPropertyChanged(nameof(Accuracy));
            NotifyPropertyChanged(nameof(GPSInfo));
            NotifyPropertyChanged(nameof(BackText));
        }

        public string BackText
        {
            get
            {
                return Util.Loader.GetString("BackTwiceText");
            }
        }


        public string Map
        {
            get
            {
                return Util.Loader.GetString("Map");
            }
        }

        public string Help
        {
            get
            {
                return Util.Loader.GetString("Help");
            }
        }

        public string Route
        {
            get
            {
                return Util.Loader.GetString("Route");
            }
        }

        public string Landmarks
        {
            get
            {
                return Util.Loader.GetString("Landmarks");
            }
        }

        public string Search
        {
            get
            {
                return Util.Loader.GetString("Search");
            }
        }

        public string Settings
        {
            get
            {
                return Util.Loader.GetString("Settings");
            }
        }

        private void Geo_OnStatusUpdate(object sender, YJMPD_UWP.Model.StatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(Status));
            });
        }

        private void Geo_OnPositionUpdate(object sender, YJMPD_UWP.Model.PositionUpdatedEventArgs e)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(Source));
                NotifyPropertyChanged(nameof(Accuracy));
            });
        }

        public string GPSInfo
        {
            get
            {
                return Util.Loader.GetString("GPSInfo");
            }
        }

        public string Status
        {
            get
            {
                switch (App.Geo.Status)
                {
                    case PositionStatus.Disabled:
                        return Util.Loader.GetString("Disabled");
                    case PositionStatus.Initializing:
                        return Util.Loader.GetString("Initializing");
                    case PositionStatus.NoData:
                        return Util.Loader.GetString("NoData");
                    default:
                    case PositionStatus.NotAvailable:
                        return Util.Loader.GetString("NotAvailable");
                    case PositionStatus.NotInitialized:
                        return Util.Loader.GetString("NotInitialized");
                    case PositionStatus.Ready:
                        return Util.Loader.GetString("Ready");
                }
            }
        }

        public string Source
        {
            get
            {
                if (App.Geo.Connected == true && App.Geo.Position != null)
                    switch (App.Geo.Position.Coordinate.PositionSource)
                    {
                        case PositionSource.Cellular:
                            return Util.Loader.GetString("Cellular");
                        case PositionSource.IPAddress:
                            return Util.Loader.GetString("IPAddress");
                        case PositionSource.Satellite:
                            return Util.Loader.GetString("Satellite");
                        case PositionSource.WiFi:
                            return Util.Loader.GetString("WiFi");
                        default:
                        case PositionSource.Unknown:
                            return Util.Loader.GetString("Unknown");

                    }
                else
                    return Util.Loader.GetString("Unknown");
            }
        }

        public string Accuracy
        {
            get
            {

                if (App.Geo.Connected == true && App.Geo.Position != null)
                    return App.Geo.Position.Coordinate.Accuracy.ToString() + "m";
                else
                    return Util.Loader.GetString("Unknown");
            }
        }

        public string Year
        {
            get
            {
                int year = DateTime.Now.Year;
                return year.ToString();
            }
        }
    }
}
