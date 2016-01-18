using Newtonsoft.Json;
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

        public enum RandomType { ALPHA, NUMERIC, ALPHANUMERIC }
        private static string AlphaSource = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
        private static string NumericSource = "0123456789";

        public static double Now { get { return (DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds; } }

        public static string Random(int length, RandomType type)
        {
            string str = "";

            string source = "";
            switch(type)
            {
                case RandomType.ALPHA:
                    source = AlphaSource;
                    break;
                case RandomType.NUMERIC:
                    source = NumericSource;
                    break;
                default:
                case RandomType.ALPHANUMERIC:
                    source = AlphaSource + NumericSource;
                    break;
            }

            Random rand = new Random();

            for(int i = 0; i<length; i++)
            {
                str += source.ElementAt(rand.Next(0,source.Length-1));
            }

            return str;
        }

        public static string MillisecondsToTime(double millis)
        {
            DateTime time = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            string timestr = time.AddMilliseconds(millis) + "";
            return timestr;
        }

        public static string Serialize<E>(E o)
        {
            return JsonConvert.SerializeObject(o);
        }

        public static E Deserialize<E>(string s)
        {
            return JsonConvert.DeserializeObject<E>(s);
        }

        //GPS

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
    }
}
