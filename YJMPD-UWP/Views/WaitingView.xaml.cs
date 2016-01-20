using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace YJMPD_UWP.Views
{
    public sealed partial class WaitingView : Page
    {
        public WaitingView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string s = e.Parameter as string;

            if (s != null)
                WaitingMsg.Text = s;
        }
    }
}
