using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace YJMPD_UWP.Helpers.Converter
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool b = (bool)value;

            if (b)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Visibility v = (Visibility)value;

            if (v == Visibility.Visible)
                return true;
            else
                return false;
        }
    }
}
