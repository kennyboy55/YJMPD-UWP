using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJMPD_UWP.Helpers;

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
            GameFound,
            PlayerJoined
        }

        public ApiHandler()
        {

        }

        public void HandleMessage(JObject o)
        {
            Command c = (Command)Enum.Parse(typeof(Command), o["command"].ToString());

            switch(c)
            {
                case Command.GameFound:
                    GameFound();
                    break;
                case Command.PlayerJoined:
                    PlayerJoined(o["msg"].ToString());
                    break;
                default:
                    //Do nothing
                    break;

            }
        }

        private void PlayerJoined(string username)
        {
            throw new NotImplementedException();
            //Event will be handled by the game manager
            App.Game.AddPlayer(username);
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
