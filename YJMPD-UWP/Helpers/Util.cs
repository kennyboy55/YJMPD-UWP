using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls.Maps;

namespace YJMPD_UWP.Helpers
{
    class Util
    {
        public enum DialogType { YESNO, OKCANCEL }

        public static double Now { get { return (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; } }

        public static string MillisecondsToTime(double millis)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            string timestr = time.AddMilliseconds(millis) + "";
            return timestr;
        }

        public static async Task<Geopoint> FindLocation(string location, Geopoint reference)
        {
            MapLocationFinderResult result = await MapLocationFinder.FindLocationsAsync(location, reference);
            MapLocation from = result.Locations.FirstOrDefault();
            Geopoint p = from.Point;
            return p;
        }

        public static async Task<MapRoute> FindWalkingRoute(Geopoint from, Geopoint to)
        {
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteAsync(from, to);
            MapRoute b = routeResult.Route;
            return b;
        }

        public static async Task<MapRoute> FindWalkingRoute(List<Geopoint> points)
        {
            MapRouteFinderResult routeResult = await MapRouteFinder.GetWalkingRouteFromWaypointsAsync(points);
            MapRoute b = routeResult.Route;
            return b;
        }

        public static async Task<MapRoute> FindWalkingRoute(string from, string to, Geopoint reference)
        {
            Geopoint f = await FindLocation(from, reference);
            Geopoint t = await FindLocation(to, reference);
            MapRoute m = await FindWalkingRoute(f, t);
            return m;
        }

        public static async Task<String> FindAddress(Geopoint p)
        {
            // Reverse geocode the specified geographic location.
            MapLocationFinderResult result =
                await MapLocationFinder.FindLocationsAtAsync(p);

            string returnstring = "";

            // If the query returns results, display the name of the town
            // contained in the address of the first result.
            if (result.Status == MapLocationFinderStatus.Success)
            {
                MapAddress address = result.Locations[0].Address;

                //returnstring = address.Street + " " + address.StreetNumber + ", " + address.Town;
                returnstring += (address.BuildingName == "" ? "" : address.BuildingName + ", ");
                returnstring += (address.Street == "" ? "" : address.Street + (address.StreetNumber == "" ? ", " : " " + address.StreetNumber + ", "));
                returnstring += address.Town;
            }

            return returnstring;
        }

        public static async Task<String> FindAddress(double latitude, double longitude)
        {
            Geopoint p = new Geopoint(new BasicGeoposition() { Latitude = latitude, Longitude = longitude });
            string address = await FindAddress(p);
            return address;
        }

        public static MapPolyline GetRouteLine(MapRoute m, Color color, int zindex, int thickness = 5)
        {
            var line = new MapPolyline
            {
                StrokeThickness = thickness,
                StrokeColor = color,
                StrokeDashed = false,
                ZIndex = zindex
            };

            if (m != null)
                line.Path = new Geopath(m.Path.Positions);

            return line;
        }

        public static MapPolyline GetRouteLine(List<BasicGeoposition> positions, Color color, int zindex, int thickness = 5)
        {
            var line = new MapPolyline
            {
                StrokeThickness = thickness,
                StrokeColor = color,
                StrokeDashed = false,
                ZIndex = zindex
            };

            line.Path = new Geopath(positions);

            return line;
        }

        public static MapPolyline GetRouteLine(BasicGeoposition p1, BasicGeoposition p2, Color color, int zindex, int thickness = 5)
        {
            var line = new MapPolyline
            {
                StrokeThickness = thickness,
                StrokeColor = color,
                StrokeDashed = false,
                ZIndex = zindex
            };

            List<BasicGeoposition> plist = new List<BasicGeoposition>();
            plist.Add(p1);
            plist.Add(p2);

            line.Path = new Geopath(plist);

            return line;
        }

        public static void ShowToastNotification(string title, string text)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastText02;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(title));
            toastTextElements[1].AppendChild(toastXml.CreateTextNode(text));

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            XmlElement audio = toastXml.CreateElement("audio");

            audio.SetAttribute("src", "ms-winsoundevent:Notification.IM");

            toastNode.AppendChild(audio);

            ToastNotification toast = new ToastNotification(toastXml);
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }

        public static async Task<bool> ShowConfirmDialog(string title, string content, DialogType type)
        {
            MessageDialog dlg = new MessageDialog(content, title);
            if (type == DialogType.YESNO)
            {
                dlg.Commands.Add(new UICommand("Yes") { Id = 0 });
                dlg.Commands.Add(new UICommand("No") { Id = 1 });
            }
            else if (type == DialogType.OKCANCEL)
            {
                dlg.Commands.Add(new UICommand("Ok") { Id = 0 });
                dlg.Commands.Add(new UICommand("Cancel") { Id = 1 });
            }

            dlg.DefaultCommandIndex = 0;
            dlg.CancelCommandIndex = 1;

            var result = await dlg.ShowAsync();

            if ((int)result.Id == 0)
                return true;
            else
                return false;
        }

        public static string TranslatedManeuver(MapRouteManeuver maneuver, int distance)
        {
            string response = "";
            bool onstreet = false;
            bool meters = true;

            distance = (int)Math.Round(distance / 5.0) * 5;

            switch (maneuver.Kind)
            {
                default:
                    response = "RouteSeeMap";
                    meters = false;
                    break;
                case MapRouteManeuverKind.End:
                    response = "RouteEnd";
                    break;
                case MapRouteManeuverKind.GoStraight:
                    response = "RouteGoStraight";
                    onstreet = true;
                    break;
                case MapRouteManeuverKind.None:
                    response = "RouteNone";
                    meters = false;
                    break;
                case MapRouteManeuverKind.Start:
                    response = "RouteStart";
                    meters = false;
                    break;
                case MapRouteManeuverKind.TurnHardLeft:
                case MapRouteManeuverKind.TurnLeft:
                    response = "RouteLeft";
                    onstreet = true;
                    break;
                case MapRouteManeuverKind.TurnHardRight:
                case MapRouteManeuverKind.TurnRight:
                    response = "RouteRight";
                    onstreet = true;
                    break;
                case MapRouteManeuverKind.TrafficCircleLeft:
                    response = "RouteTrafficCircleLeft";
                    onstreet = true;
                    break;
                case MapRouteManeuverKind.TrafficCircleRight:
                    response = "RouteTrafficCircleRight";
                    onstreet = true;
                    break;
                case MapRouteManeuverKind.TurnKeepLeft:
                case MapRouteManeuverKind.TurnLightLeft:
                    response = "RouteKeepLeft";
                    break;
                case MapRouteManeuverKind.TurnKeepRight:
                case MapRouteManeuverKind.TurnLightRight:
                    response = "RouteKeepRight";
                    break;
                case MapRouteManeuverKind.UTurnLeft:
                case MapRouteManeuverKind.UTurnRight:
                    response = "RouteUTurn";
                    break;
            }

            if (maneuver.StreetName == "")
                onstreet = false;

            if (distance < 10)
                meters = false;


            if (onstreet)
                response += " " + "RouteOn" + " " + maneuver.StreetName;

            if (meters)
                response = "RouteIn" + " " + distance + "m" + " " + response.ToLower();

            return response;
        }
    }
}
