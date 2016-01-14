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
    }
}
