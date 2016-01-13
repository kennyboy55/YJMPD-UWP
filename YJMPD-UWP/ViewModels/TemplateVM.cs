using YJMPD_UWP.Helpers;
using System;
using System.ComponentModel;
using Windows.UI.Core;

namespace YJMPD_UWP.ViewModels
{
    public class TemplateVM : INotifyPropertyChanged
    {
        protected CoreDispatcher dispatcher;

        public TemplateVM(string title)
        {
            dispatcher = App.Dispatcher;
            Settings.OnLanguageUpdate += Settings_OnLanguageUpdate;

            if (App.MainPage != null)
                App.MainPage.Title = title;
        }

        private void Settings_OnLanguageUpdate(EventArgs e)
        {
            dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                UpdatePropertiesToNewLanguage();
            });
        }

        protected virtual void UpdatePropertiesToNewLanguage()
        {
            //to implement in underlying class
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
