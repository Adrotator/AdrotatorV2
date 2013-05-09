using System;
using System.Windows;
using System.Device.Location;

namespace InneractiveAdLocation
{
    public class IaLocationEventArgs : EventArgs
    {
        public IaLocationEventArgs(string location)
        {
            this.location = location;
        }

        public string location;
    }

    public class IaLocationClass
    {
        // Location done event
        public event EventHandler<IaLocationEventArgs> Done;

        private GeoCoordinateWatcher gcWatcher = null;
        private string locationStr = null;

        // Raise the Done event
        private void NotifyDone(GeoPositionStatusChangedEventArgs e)
        {
            if (Done != null)
            {
                IaLocationEventArgs locationArgs = null;

                if (e.Status == GeoPositionStatus.Ready &&
                    locationStr != null && locationStr.Length > 0)
                {
                    locationArgs = new IaLocationEventArgs(locationStr);
                }

                Done(this, locationArgs);
            }
        }

        /// <summary>
        /// watcher_StatusChanged
        /// </summary>
        private void gcWatcher_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyStatusChanged(e));
        }

        /// <summary>
        /// MyStatusChanged
        /// </summary>
        private void MyStatusChanged(GeoPositionStatusChangedEventArgs e)
        {
            switch (e.Status)
            {
                case GeoPositionStatus.Disabled:
                case GeoPositionStatus.NoData:
                case GeoPositionStatus.Ready:
                    // Stop watch location
                    gcWatcher.Stop();
                    // Notify
                    NotifyDone(e);
                    break;
                case GeoPositionStatus.Initializing:
                default:
                    break;
            }
        }

        /// <summary>
        /// watcher_PositionChanged
        /// </summary>
        private void gcWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() => MyPositionChanged(e));
        }

        /// <summary>
        /// MyPositionChanged
        /// </summary>
        private void MyPositionChanged(GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            locationStr = e.Position.Location.Latitude.ToString("0.0000") + "," + e.Position.Location.Longitude.ToString("0.0000");
        }

        /// <summary>
        /// StartWatchLocation
        /// </summary>
        public void StartWatchLocation()
        {
            try
            {
                if (gcWatcher == null)
                {
                    gcWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                    gcWatcher.MovementThreshold = 20; // 20 meters. 
                    gcWatcher.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(gcWatcher_StatusChanged);
                    gcWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcWatcher_PositionChanged);
                }

                gcWatcher.Start();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.ToString());
            }
        }
    }
}
