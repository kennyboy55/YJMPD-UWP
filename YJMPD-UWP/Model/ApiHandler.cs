using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Views;

namespace YJMPD_UWP.Model
{
    public class ApiHandler
    {
        public delegate void OnGameFoundHandler(object sender, EventArgs e);
        public event OnGameFoundHandler OnGameFound;

        public enum Command
        {
            Hi,
            Name,
            Picture,
            Msg,
            GameFound,
            PlayerJoined,
            PlayerRemoved
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
                case Command.GameFound:
                    GameFound();
                    break;
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
                        Debug.WriteLine("Selected player taking picture");
                        App.Game.Selected = true;
                        App.Navigate(typeof(PhotoView));
                    }
                    else
                        App.Navigate(typeof(WaitingView));
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

        private void GameFound()
        {
            throw new NotImplementedException();
            if (OnGameFound == null) return;

            OnGameFound(this, new EventArgs());
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
        public async Task<bool> SearchGame()
        {
            JObject obj = JObject.FromObject(new
            {
                command = Command.Name.ToString(),
                name = Settings.Username
            });
            Debug.WriteLine(obj.ToString(Formatting.None));
            await App.Network.Write(obj.ToString(Formatting.None));

            return true;
        }
    }
}
