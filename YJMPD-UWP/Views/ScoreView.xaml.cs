using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
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
