using System.Collections.Generic;

namespace YJMPD_UWP.Model.Object
{
    public class Statistics
    {
        
        public Statistics()
        {
            Points = new List<double>();
            Distance = 0;
            Matches = 0;
            Leader = 0;
        }

        public List<double> Points { get; set; }
        public double Distance { get; set; }
        public int Matches { get; set; }
        public int Leader { get; set; }

        public void AddPoints(double points)
        {
            Points.Add(points);
        }

    }
}
