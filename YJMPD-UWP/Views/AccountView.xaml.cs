using Windows.UI.Xaml.Controls;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class AccountView : Page
    {
        AccountVM accountvm;

        public AccountView()
        {
            accountvm = new AccountVM();
            this.DataContext = accountvm;
            this.InitializeComponent();
        }
    }
}
