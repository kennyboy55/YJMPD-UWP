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
    }
}
