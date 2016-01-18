using System.Linq;
using YJMPD_UWP.Helpers;
using YJMPD_UWP.Model.Object;

namespace YJMPD_UWP.ViewModels
{
    public class StatisticsVM : TemplateVM
    {
        Statistics st;

        public StatisticsVM() : base("Statistics")
        {
            st = Settings.Statistics;
        }

        public string Information
        {
            get
            {
                return "This page holds all the statistics for " + Settings.Username;
            }
        }

        public string Distance
        {
            get
            {
                return st.Distance + "km";
            }
        }

        public string LeaderCount
        {
            get
            {
                return st.Leader + "";
            }
        }

        public string MatchesCount
        {
            get
            {
                return st.Matches + "";
            }
        }

        public string PointsTotal
        {
            get
            {
                return st.Points.Sum() + "";
            }
        }

        public string PointsAverage
        {
            get
            {
                if (st.Points.Count < 1)
                    return "0";
                else
                    return (st.Points.Sum() / st.Matches) + "";
            }
        }

        public string PointsMax
        {
            get
            {
                if (st.Points.Count < 1)
                    return "0";
                else
                    return st.Points.Max() + "";
            }
        }

        public string PointsMin
        {
            get
            {
                if (st.Points.Count < 1)
                    return "0";
                else
                    return st.Points.Min() + "";
            }
        }
    }
}
