using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class MatchView : Page
    {
        MatchVM matchvm;

        public MatchView()
        {
            matchvm = new MatchVM();
            this.DataContext = matchvm;
            this.InitializeComponent();
        }

        private void StartMatchButton_Click(object sender, RoutedEventArgs e)
        {
            App.Game.Start();
        }

        private void StopMatchButton_Click(object sender, RoutedEventArgs e)
        {
            App.Game.Stop();
        }
    }
}
