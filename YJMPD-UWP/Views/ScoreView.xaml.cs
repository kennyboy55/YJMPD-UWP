using System;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class ScoreView : Page
    {
        ScoreVM scorevm;

        public ScoreView()
        {
            scorevm = new ScoreVM();
            this.DataContext = scorevm;
            this.InitializeComponent();
        }

        private void ReadyButton_Click(object sender, RoutedEventArgs e)
        {
            Ready();
        }

        private async void Ready()
        {
            ReadyCheck.Visibility = Visibility.Visible;
            ReadyButton.IsEnabled = false;

            App.Api.Ready();

            await Task.Delay(TimeSpan.FromMilliseconds(500));

            App.Game.MoveToWaiting();
            App.Navigate(typeof(WaitingView), "Waiting on other players...");
        }
    }
}
