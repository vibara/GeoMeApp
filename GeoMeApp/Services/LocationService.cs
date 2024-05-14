using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services
{
    internal class LocationService : ILocationService
    {

        private Location? Location { get; set; }
        public double LocationUpdateSeconds { get; set; } = 5;
        private CancellationTokenSource? _cancelTokenSource;
        private bool _isCheckingLocation;

        public Location? GetLocation()
        {
            Task<Location?> lastLocation = RequestLocation();
            lastLocation.ConfigureAwait(false);
            return Location;
        }

        protected async Task<Location?> RequestLocation()
        {
            try
            {
                _isCheckingLocation = true;
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cancelTokenSource = new CancellationTokenSource();
                Location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                Debug.Print($"Request location: Feature not supported, message: {fnsEx.Message}");                
            }
            catch (FeatureNotEnabledException fneEx)
            {
                Debug.Print($"Request location: Feature not enabled, message: {fneEx.Message}");
            }
            catch (PermissionException pEx)
            {
                Debug.Print($"Request location: Permission exception: {pEx.Message}");
            }
            catch (Exception ex)
            {
                Debug.Print($"Request location: General exception: {ex.Message}");
            }
            finally
            {
                _cancelTokenSource?.Cancel();
            }
            return Location;
        }

        protected void CancelLocationRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            {
                _cancelTokenSource.Cancel();
            }
        }
    }
}
