using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Views;

namespace YJMPD_UWP.Model
{
    public class ApiHandler
    {

        public enum Command
        {
            Hi,
            Name,
            Picture,
            Msg,
            PlayerJoined,
            PlayerRemoved,
            PictureUrl,
            DestinationReached,
            GameEnded,
            PlayerReady
        }

        public ApiHandler()
        {

        }

        public void HandleMessage(JObject o)
        {
            Debug.WriteLine(o.ToString());
            Command c = (Command)Enum.Parse(typeof(Command), o["command"].ToString());

            switch (c)
            {
                case Command.PlayerJoined:
                    Debug.WriteLine("Played joined");
                    PlayerJoined(o[Command.PlayerJoined.ToString()].ToString());
                    break;
                case Command.PlayerRemoved:
                    Debug.WriteLine("Played removed");
                    PlayerRemoved(o[Command.PlayerRemoved.ToString()].ToString());
                    break;
                case Command.Picture:
                    if (o["selected"].ToObject<bool>() == true)
                    {
                        App.Game.SetSelected(true);
                        App.Navigate(typeof(PhotoView));
                    }
                    else
                        App.Navigate(typeof(WaitingView), "Waiting on photo...");

                    App.Game.MoveToWaiting();
                    break;
                case Command.PictureUrl:
                    if (!App.Game.Selected)
                        App.Photo.SetPhoto(o[Command.PictureUrl.ToString()].ToString());

                    double lat = (double)o["lat"];
                    double lon = (double)o["lon"];

                    BasicGeoposition bgps = new BasicGeoposition() { Latitude = lat, Longitude = lon };

                    App.Game.MoveToStarted(bgps);
                    break;
                case Command.GameEnded:
                    string winner = o["winner"].ToString();

                    if (winner == Settings.Username)
                        winner = "You";

                    Util.ShowToastNotification(winner + " won!", "Press Ready or Leave");

                    JArray players = (JArray)o["players"];

                    foreach(JToken pl in players)
                    {
                        string username = o["username"].ToString();
                        double points = (double)o["points"];
                        double pointstotal = (double)o["pointstotal"];

                        App.Game.UpdatePlayer(username, pointstotal, points);
                    }

                    App.Game.StopMatch();
                    break;
                default:
                    //Do nothing
                    break;

            }
        }

        private void PlayerJoined(string username)
        {
            //Event will be handled by the game manager
            App.Game.AddPlayer(username);
        }

        private void PlayerRemoved(string username)
        {
            //Event will be handled by the game manager
            App.Game.RemovePlayer(username);
        }

        public JObject Message(Command c, string msg)
        {
            return JObject.FromObject(new
            {
                command = c.ToString(),
                msg = msg
            });
        }

        //API stuff
        public async Task<bool> JoinGame()
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.Name.ToString(),
                name = Settings.Username,
                lon = App.Geo.Position.Coordinate.Point.Position.Longitude,
                lat = App.Geo.Position.Coordinate.Point.Position.Latitude
            });
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }

        public async Task<bool> LeaveGame()
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.PlayerRemoved.ToString(),
                name = Settings.Username
            });
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }

        public async Task<bool> SendPhoto(string url)
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.PictureUrl.ToString(),
                pictureurl = url,
                lon = App.Geo.Position.Coordinate.Point.Position.Longitude,
                lat = App.Geo.Position.Coordinate.Point.Position.Latitude
            });
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }

        public async Task<bool> DestinationReached()
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.DestinationReached.ToString(),
                username = Settings.Username
            });
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }

        public async Task<bool> Ready()
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.PlayerReady.ToString(),
                ready = true
            });
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }
    }
}
