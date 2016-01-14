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
    }
}
