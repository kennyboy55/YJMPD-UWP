using System;
using System.Diagnostics;
using Windows.Foundation.Collections;
using Windows.Storage;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.Helpers
{
    static class Settings
    {
        public static IPropertySet Values = ApplicationData.Current.LocalSettings.Values;

        static Settings()
        {
            //Define default settings here
            Values["hostname"] = "imegumii.space";
            Values["port"] = "3333";

            if (Values["username"] == null)
                Values["username"] = "Anon_" + Util.Random(5, Util.RandomType.ALPHANUMERIC);

            if (Values["statistics"] == null)
                Values["statistics"] = Util.Serialize(new Statistics());


            st = Util.Deserialize<Statistics>(Values["statistics"] as string);
        }


        private static Statistics st;
        public static Statistics Statistics
        {
            get
            {
                return st;
            }
        }


        public static string Username
        {
            get
            {
                return Values["username"] as string;
            }
            set
            {
                Values["username"] = value;
            }
        }
    }
}
