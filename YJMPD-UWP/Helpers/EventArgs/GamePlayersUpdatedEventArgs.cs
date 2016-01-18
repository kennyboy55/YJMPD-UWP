using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class GamePlayersUpdatedEventArgs : System.EventArgs
    {
        public string Username { get; private set; }
        public double Points { get; private set; }

        public GamePlayersUpdatedEventArgs(string username, double points)
        {
            Username = username;
            Points = points;
        }
    }
}
