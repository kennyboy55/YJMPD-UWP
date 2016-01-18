using Windows.Foundation.Collections;
using Windows.Storage;

namespace YJMPD_UWP.Helpers
{
    static class Settings
    {
        public static IPropertySet Values = ApplicationData.Current.LocalSettings.Values;

        static Settings()
        {
            //Define default settings here
            Values["hostname"] = "imegumii.space";
        }
    }
}
