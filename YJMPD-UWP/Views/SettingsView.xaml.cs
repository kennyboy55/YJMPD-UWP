using YJMPD_UWP.ViewModels;
using Windows.UI.Xaml.Controls;

namespace YJMPD_UWP.Views
{
    public sealed partial class SettingsView : Page
    {
        SettingsVM settingsvm;

        public SettingsView()
        {
            settingsvm = new SettingsVM();
            this.DataContext = settingsvm;
            this.InitializeComponent();
        }
    }
}
