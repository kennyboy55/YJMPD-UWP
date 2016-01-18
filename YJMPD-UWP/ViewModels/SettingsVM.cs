using YJMPD_UWP.Helpers;

namespace YJMPD_UWP.ViewModels
{
    public class SettingsVM : TemplateVM
    {
        public SettingsVM() : base("Settings")
        {

        }

        public string Username
        {
            get
            {
                return Settings.Username;
            }
            set
            {
                Settings.Username = value;
            }
        }
    }
}
