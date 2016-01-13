using YJMPD_UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace YJMPD_UWP.Views
{
    public sealed partial class HelpView : Page
    {
        HelpVM helpvm;

        public HelpView()
        {
            helpvm = new HelpVM();
            this.DataContext = helpvm;
            this.InitializeComponent();
        }
    }
}
