using System;

namespace YJMPD_UWP.ViewModels
{
    public class MainPageVM : TemplateVM
    {
        public MainPageVM() : base("Loading")
        {
            
        }

        public string GameState
        {
            get
            {
                return "N/A";
            }
        }

        public string People
        {
            get
            {
                return "0/0";
            }
        }

        public bool GameVisible
        {
            get
            {
                return true;
            }
        }

        public string Year
        {
            get
            {
                int year = DateTime.Now.Year;
                return year.ToString();
            }
        }
    }
}
