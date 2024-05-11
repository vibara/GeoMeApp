using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services
{
    internal class LocationService : ILocationService
    {
        public Location? Location { get; private set; }
        public double LocationUpdateSeconds { get; set; } = 5;
        private CancellationTokenSource? _cancelTokenSource;
        private bool _isCheckingLocation;
        private Timer? _locationUpdateTimer;

        private LocationService()
        {

        }
        protected async void RequestLocation(object state)
        {
            try
            {
                _isCheckingLocation = true;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

                _cancelTokenSource = new CancellationTokenSource();
                Location = await Geolocation.Default.GetLastKnownLocationAsync();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
            finally
            {
                _cancelTokenSource?.Cancel();
            }
        }

        protected void CancelLocationRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            {
                _cancelTokenSource.Cancel();
            }

        }

        protected void StartLocationUpdates()
        {
            StopLocatopnUpdates();
            _locationUpdateTimer = new Timer(RequestLocation, null, TimeSpan.Zero, TimeSpan.FromSeconds(LocationUpdateSeconds));
        }

        protected void StopLocatopnUpdates()
        {
            if (_locationUpdateTimer != null)
            {
                _locationUpdateTimer?.Dispose();
                _locationUpdateTimer = null;
            }
        }
        protected void OnSleep()
        {
            StopLocatopnUpdates();
        }
        protected void OnResume()
        {
            StartLocationUpdates();
        }
    }
}
