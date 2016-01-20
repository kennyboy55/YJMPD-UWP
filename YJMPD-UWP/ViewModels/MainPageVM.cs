using System;
using YJMPD_UWP.Helpers;

namespace YJMPD_UWP.ViewModels
{
    public class MainPageVM : TemplateVM
    {
        public MainPageVM() : base("Loading")
        {
            App.Game.OnStatusUpdate += Game_OnStatusUpdate;
            App.Game.OnPlayersUpdate += Game_OnPlayersUpdate;
        }

        private void Game_OnPlayersUpdate(object sender, Helpers.EventArgs.GamePlayersUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(Players));
            });
        }

        private void Game_OnStatusUpdate(object sender, Helpers.EventArgs.GameStatusUpdatedEventArgs e)
        {
            dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                NotifyPropertyChanged(nameof(GameState));
                NotifyPropertyChanged(nameof(GameVisible));
            });
        }

        public string GameState
        {
            get
            {
                return App.Game.Status.ToString().UppercaseFirst();
            }
        }

        public string Players
        {
            get
            {
                if (App.Game.Players.Count == 1)
                    return "1 player";
                else
                    return App.Game.Players.Count + " players";
            }
        }

        public bool GameVisible
        {
            get
            {
                return App.Game.Status != Model.GameHandler.GameStatus.STOPPED;
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
