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

            Task.Delay(TimeSpan.FromMilliseconds(1500));

            App.Navigate(typeof(WaitingView));
        }
    }
}
