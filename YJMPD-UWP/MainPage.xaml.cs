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
using Windows.Devices.Geolocation;

namespace YJMPD_UWP
{
    public sealed partial class MainPage : Page
    {
        double bptime;
        double lastbptime;



        public Frame ContentFrame
        {
            get
            {
                return Frame;
            }
        }

        public string Title { get { return PageTitle.Text; } set { PageTitle.Text = value; } }

        MainPageVM mainpagevm;

        public MainPage()
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

            bptime = Util.Now;

            mainpagevm = new MainPageVM();
            this.DataContext = mainpagevm;

            this.InitializeComponent();

            Frame.Navigated += Frame_Navigated;
            Frame.Navigate(typeof(GameView));
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (e.Handled) return;

            if (App.Game.Status == Model.GameHandler.GameStatus.STARTED)
            {
                e.Handled = true;
                return;
            }

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
                e.Handled = true;
                ShowHideBackMessage();
                return;
            }

            BackMessage.Visibility = Visibility.Collapsed;
        }

        public async Task<String> ShowHideBackMessage()
        {
            BackMessage.Visibility = Visibility.Visible;
            await Task.Delay(TimeSpan.FromSeconds(2));
            BackMessage.Visibility = Visibility.Collapsed;
            return "success";
        }

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
                case "gameview":
                    NavList.SelectedIndex = 0;
                    break;
                case "statisticsview":
                    NavList.SelectedIndex = 1;
                    break;
                case "aboutview":
                    NavList.SelectedIndex = 2;
                    break;
                case "settingsview":
                    NavList.SelectedIndex = 3;
                    break;
                default:
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

            if (NavListHome.IsSelected)
            {
                if (Frame.BackStack.Count >= 1)
                    Frame.BackStack.Clear();
                Frame.Navigate(typeof(GameView));
            }
            else
            if (NavListStatistics.IsSelected)
                Frame.Navigate(typeof(StatisticsView));
            else
            if (NavListAbout.IsSelected)
                Frame.Navigate(typeof(AboutView));
            else
            if (NavListSettings.IsSelected)
                Frame.Navigate(typeof(SettingsView));
        }

        private void NavList_Tapped(object sender, TappedRoutedEventArgs e)
        {
            NavView.IsPaneOpen = false;
        }

        private void BackToGame_Click(object sender, RoutedEventArgs e)
        {
            NavView.IsPaneOpen = false;
            App.Game.BackToGame();
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
    }
}
