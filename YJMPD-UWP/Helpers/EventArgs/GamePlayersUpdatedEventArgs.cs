using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.Helpers.EventArgs
{
    public class GamePlayersUpdatedEventArgs : System.EventArgs
    {
        public Player Player { get; private set; }

        public GamePlayersUpdatedEventArgs(Player player)
        {
            Player = player;
        }
    }
}
