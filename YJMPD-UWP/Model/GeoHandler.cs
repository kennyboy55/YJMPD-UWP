using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Devices.Geolocation.Geofencing;
using Windows.System;
using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class GeoHandler
    {
        private Geolocator geo;

        private PositionStatus _status;
        public PositionStatus Status { get { return _status; } }

        private Geoposition _position;
        public Geoposition Position { get { return _position; } }

        public bool? Connected { get; private set; }

        private List<Geoposition> _history;
        public List<Geoposition> History
        {
            get
            {
                return _history;
            }
        }

        //Events
        public delegate void OnPositionUpdateHandler(object sender, PositionUpdatedEventArgs e);
        public event OnPositionUpdateHandler OnPositionUpdate;

        public delegate void OnStatusUpdateHandler(object sender, PositionStatusUpdatedEventArgs e);
        public event OnStatusUpdateHandler OnStatusUpdate;

        public GeoHandler()
        {
            _status = PositionStatus.NotInitialized;
            Connected = false;
            _history = new List<Geoposition>();
            StartTracking();
        }

        public async void ForceRefresh()
        {
            if (geo == null)
                await StartTracking();
            else
                _position = await geo.GetGeopositionAsync();
        }

        public async void TryConnectIfNull()
        {
            if (geo == null)
                await StartTracking();
        }

        public void ClearHistory()
        {
            _history.Clear();
        }

        public async Task<String> StartTracking()
        {
            // Request permission to access location
            if (Status != PositionStatus.NotAvailable && Status != PositionStatus.NotInitialized)
                return "Already Connected";

            var accessStatus = await Geolocator.RequestAccessAsync();

            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    geo = new Geolocator
                    {
                        DesiredAccuracy = PositionAccuracy.High,
                        MovementThreshold = 3
                        //ReportInterval = 1500
                    };

                    ClearHistory();

                    Connected = true;

                    geo.PositionChanged += Geo_PositionChanged;
                    geo.StatusChanged += Geo_StatusChanged;

                    GeofenceMonitor.Current.Geofences.Clear();

                    _position = await geo.GetGeopositionAsync();

                    return "Connected";

                case GeolocationAccessStatus.Denied:
                    Connected = false;
                    _status = PositionStatus.NotAvailable;
                    bool result = await Launcher.LaunchUriAsync(new Uri("ms-settings:privacy-location"));
                    return "Denied";

                default:
                case GeolocationAccessStatus.Unspecified:
                    Connected = false;
                    _status = PositionStatus.NotAvailable;
                    return "Error";
            }
        }

        private void Geo_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            if (args.Status == PositionStatus.Disabled)
            {
                Connected = false;
                _position = null;
            }
            else if (!(bool)Connected)
                Connected = true;

            UpdateStatus(args.Status);
        }

        private void Geo_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            if (_history.Count > 0)
                UpdatePosition(_history.Last(), args.Position);
            else
            {
                _position = args.Position;
                UpdatePosition(args.Position, args.Position);
            }

            _history.Add(args.Position);
        }

        private void UpdateStatus(PositionStatus s)
        {
            _status = s;

            if (OnStatusUpdate == null) return;

            OnStatusUpdate(this, new PositionStatusUpdatedEventArgs(s));
        }

        private void UpdatePosition(Geoposition old, Geoposition newp)
        {
            _position = newp;

            if (OnPositionUpdate == null) return;

            OnPositionUpdate(this, new PositionUpdatedEventArgs(old, newp));
        }
    }
}
