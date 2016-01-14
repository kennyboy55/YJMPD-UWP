using Windows.UI.Xaml.Controls;
using YJMPD_UWP.ViewModels;

namespace YJMPD_UWP.Views
{
    public sealed partial class StatisticsView : Page
    {
        StatisticsVM statisticsvm;

        public StatisticsView()
        {
            statisticsvm = new StatisticsVM();
            this.DataContext = statisticsvm;
            this.InitializeComponent();
        }
    }
}
