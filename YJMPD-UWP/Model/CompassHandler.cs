using YJMPD_UWP.Helpers;
using System;
using Windows.Devices.Sensors;
using YJMPD_UWP.Helpers.EventArgs;

namespace YJMPD_UWP.Model
{
    public class CompassHandler
    {
        public delegate void OnHeadingUpdateHandler(object sender, HeadingUpdatedEventArgs e);
        public event OnHeadingUpdateHandler OnHeadingUpdate;

        public delegate void OnHeadingUpdateSlowHandler(object sender, HeadingUpdatedEventArgs e);
        public event OnHeadingUpdateSlowHandler OnSlowHeadingUpdated;

        private Compass comp;

        private CompassReading hdn;
        public CompassReading Heading { get { return hdn; } }

        private CompassReading lastreading;
        private double lastreadingtime;

        public CompassHandler()
        {
            comp = Compass.GetDefault();

            // Assign an event handler for the compass reading-changed event
            if (comp != null)
            {
                // Establish the report interval for all scenarios
                uint minReportInterval = comp.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                comp.ReportInterval = reportInterval;
                comp.ReadingChanged += Comp_ReadingChanged;
                hdn = comp.GetCurrentReading();
            }
        }

        private void Comp_ReadingChanged(Compass sender, CompassReadingChangedEventArgs args)
        {
            UpdateHeading(args.Reading);
            UpdateSlowHeading(args.Reading);
        }

        private void UpdateHeading(CompassReading r)
        {
            hdn = r;

            //Make sure someone is listening
            if (OnHeadingUpdate == null) return;

            OnHeadingUpdate(this, new HeadingUpdatedEventArgs(r));
        }

        private void UpdateSlowHeading(CompassReading r)
        {
            if (lastreading == null) { lastreading = r; lastreadingtime = Util.Now; }

            if (Math.Abs(r.HeadingMagneticNorth - lastreading.HeadingMagneticNorth) > 10 && Util.Now - lastreadingtime > 25)
            {
                lastreading = r;
                lastreadingtime = Util.Now;

                //Make sure someone is listening
                if (OnSlowHeadingUpdated == null) return;

                OnSlowHeadingUpdated(this, new HeadingUpdatedEventArgs(r));
            }
        }
    }
}
