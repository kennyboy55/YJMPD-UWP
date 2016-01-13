using YJMPD_UWP.Helpers;
using YJMPD_UWP.ViewModels;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace YJMPD_UWP.Views
{
    public sealed partial class SettingsView : Page
    {
        SettingsVM settingsvm;

        public SettingsView()
        {
            this.InitializeComponent();
            settingsvm = new SettingsVM();
            this.DataContext = settingsvm;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (Settings.CurrentLanguage)
            {
                default:
                    Debug.WriteLine("Unsupported language: " + Settings.CurrentLanguage);
                    Language.SelectedIndex = 0;
                    break;
                case "en":
                    Language.SelectedIndex = 0;
                    break;
                case "nl":
                    Language.SelectedIndex = 1;
                    break;
                case "de":
                    Language.SelectedIndex = 2;
                    break;
                case "ja":
                    Language.SelectedIndex = 3;
                    break;
            }
        }

        private void Language_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (Language.SelectedIndex)
            {
                default:
                    Language.SelectedIndex = 0;
                    break;
                case 0:
                    if (Settings.CurrentLanguage != "en")
                        Settings.ChangeLanguage("en");
                    break;
                case 1:
                    if (Settings.CurrentLanguage != "nl")
                        Settings.ChangeLanguage("nl");
                    break;
                case 2:
                    if (Settings.CurrentLanguage != "de")
                        Settings.ChangeLanguage("de");
                    break;
                case 3:
                    if (Settings.CurrentLanguage != "ja")
                        Settings.ChangeLanguage("ja");
                    break;
            }
        }

        private async void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            bool confirm = await Util.ShowConfirmDialog(Util.Loader.GetString("Reset"), Util.Loader.GetString("ResetConfirmation"), Util.DialogType.YESNO);

            if (confirm)
            {
                ResetProgress.IsActive = true;
                Language.SelectedIndex = 0;
                ResetProgress.IsActive = false;
            }
        }
    }
}
