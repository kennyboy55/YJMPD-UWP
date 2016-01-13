using YJMPD_UWP.Helpers;

namespace YJMPD_UWP.ViewModels
{
    public class HelpVM : TemplateVM
    {
        public HelpVM() : base(Util.Loader.GetString("Help"))
        {

        }

        protected override void UpdatePropertiesToNewLanguage()
        {
            NotifyPropertyChanged(nameof(HelpItem1Header));
            NotifyPropertyChanged(nameof(HelpItem1Text));

            NotifyPropertyChanged(nameof(HelpItem2Header));
            NotifyPropertyChanged(nameof(HelpItem2Text));

            NotifyPropertyChanged(nameof(HelpItem3Header));
            NotifyPropertyChanged(nameof(HelpItem3Text));
        }

        public string HelpItem1Header
        {
            get
            {
                return Util.Loader.GetString("HelpItem1Header");
            }
        }

        public string HelpItem1Text
        {
            get
            {
                return Util.Loader.GetString("HelpItem1Text");
            }
        }

        public string HelpItem2Header
        {
            get
            {
                return Util.Loader.GetString("HelpItem2Header");
            }
        }

        public string HelpItem2Text
        {
            get
            {
                return Util.Loader.GetString("HelpItem2Text");
            }
        }

        public string HelpItem3Header
        {
            get
            {
                return Util.Loader.GetString("HelpItem3Header");
            }
        }

        public string HelpItem3Text
        {
            get
            {
                return Util.Loader.GetString("HelpItem3Text");
            }
        }
    }
}
