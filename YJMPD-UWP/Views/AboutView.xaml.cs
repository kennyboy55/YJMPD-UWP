using System;
using YJMPD_UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace YJMPD_UWP.Views
{
    public sealed partial class AboutView : Page
    {
        AboutVM aboutvm;

        public AboutView()
        {
            aboutvm = new AboutVM();
            this.DataContext = aboutvm;
            this.InitializeComponent();
        }

        private void YorickGit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://github.com/yorickr"));
        }
        private void KennethGit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://github.com/kennyboy55"));
        }
        private void JancoGit_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://github.com/jancoow"));
        }

        private void YorickSite_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://imegumii.space"));
        }
        private void KennethSite_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("http://kvewijk.nl"));
        }
        private void JancoSite_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Windows.System.Launcher.LaunchUriAsync(new Uri("https://jancokock.me"));
        }
    }
}
