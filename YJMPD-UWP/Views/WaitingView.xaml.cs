using System;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace YJMPD_UWP.Views
{
    public sealed partial class WaitingView : Page
    {
        public WaitingView()
        {
            this.InitializeComponent();
            StartWait();
        }

        private async void StartWait()
        {
            await Task.Delay(TimeSpan.FromSeconds(2));
            App.Navigate(typeof(PhotoView));
        }
    }
}
