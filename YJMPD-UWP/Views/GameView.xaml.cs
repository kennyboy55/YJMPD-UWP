using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class GameView : Page
    {
        GameVM gamevm;

        public GameView()
        {
            gamevm = new GameVM();
            this.DataContext = gamevm;
            this.InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            App.Game.StartGame();
        }

        private void StopGameButton_Click(object sender, RoutedEventArgs e)
        {
            App.Game.StopGame();
        }
    }
}
