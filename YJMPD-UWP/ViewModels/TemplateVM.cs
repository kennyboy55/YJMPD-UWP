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

            if (App.MainPage != null)
                App.MainPage.Title = title;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
