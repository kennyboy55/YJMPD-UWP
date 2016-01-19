using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.ViewModels
{
    public class ScoreVM : TemplateVM
    {
        private List<Player> players;

        public ScoreVM() : base("Scores")
        {
            this.players = App.Game.Players.OrderBy(p => p.Points).ToList();
            App.Game.OnPlayersUpdate += Game_OnPlayersUpdate;
        }

        private void Game_OnPlayersUpdate(object sender, Helpers.EventArgs.GamePlayersUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                this.players = App.Game.Players.OrderBy(p => p.Points).ToList();
                NotifyPropertyChanged(nameof(Players));
                NotifyPropertyChanged(nameof(PlayersCount));
            });
        }

        public List<Player> Players
        {
            get
            {
                return new List<Player>(players);
            }
        }

        public string PlayersCount
        {
            get
            {
                return "There are currently " + App.Game.Players.Count + " players in the match.";
            }
        }
    }
}
