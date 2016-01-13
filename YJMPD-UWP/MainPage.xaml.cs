using YJMPD_UWP.Helpers;
using YJMPD_UWP.ViewModels;
using YJMPD_UWP.Views;
using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace YJMPD_UWP
{
    public sealed partial class MainPage : Page
    {
        double bptime;
        double lastbptime;

        public MainPage()
        {
            this.InitializeComponent();
            Frame.Navigated += Frame_Navigated;
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            bptime = Util.Now;

            this.DataContext = new MainPageVM();
            Frame.Navigate(typeof(MapView));
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled) return;

            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
                return;
            }

            lastbptime = bptime;
            bptime = Util.Now;

            if (bptime - lastbptime > 2000)
            {
                ShowHideBackMessage();
                e.Handled = true;
            }
        }

        public async Task<String> ShowHideBackMessage()
        {
            BackMessage.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(2));
            BackMessage.Visibility = Visibility.Collapsed;
            return "success";
        }

        public void Navigate(Type type)
        {
            Frame.Navigate(type);
        }

        public void Navigate(Type type, object param)
        {
            Frame.Navigate(type, param);
        }

        public string Title { get { return PageTitle.Text; } set { PageTitle.Text = value; } }

        public void NavButton_Click(object sender, RoutedEventArgs arg)
        {
            NavView.IsPaneOpen = !NavView.IsPaneOpen;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            //Dirty Hack
            string pagename = e.SourcePageType.ToString().Split('.').Last();

            NavList.SelectedIndex = -1;

            switch (pagename.ToLower())
            {
                default:
                    PageTitle.Text = "Nav City Breda";
                    break;
                case "helpview":
                    NavList.SelectedIndex = 3;
                    break;
                case "settingsview":
                    NavList.SelectedIndex = 5;
                    break;
                case "mapview":
                    NavList.SelectedIndex = 0;
                    break;
                case "routeview":
                case "routedetailview":
                    NavList.SelectedIndex = 1;
                    break;
                case "landmarkdetailview":
                case "landmarkview":
                    NavList.SelectedIndex = 2;
                    break;
                case "searchview":
                    NavList.SelectedIndex = 4;
                    break;
            }

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                Frame.CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void NavList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavView.IsPaneOpen = false;
        }

        private void NavList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavView.IsPaneOpen = false;
        }

        private void Content_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X > 20)
            {
                NavView.IsPaneOpen = true;
            }
        }

        private void Pane_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            if (e.Cumulative.Translation.X < -20)
            {
                NavView.IsPaneOpen = false;
            }
        }

        private void GPSRefresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            App.Geo.ForceRefresh();
        }
    }
}
