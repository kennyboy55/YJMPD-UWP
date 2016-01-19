namespace YJMPD_UWP.Model.Object
{
    public class Player
    {
        public string Username { get; private set; }
        public double PointsTotal { get; private set; }
        public double Points { get; private set; }

        public Player(string username)
        {
            Username = username;
            Points = 0;
            PointsTotal = 0;
        }

        public Player(string username, double pointstotal)
        {
            Username = username;
            PointsTotal = pointstotal;
            Points = 0;
        }

        public void Reset()
        {
            Points = 0;
        }

        public void Update(double pointstotal, double points)
        {
            PointsTotal = pointstotal;
            Points = points;
        }
    }
}
