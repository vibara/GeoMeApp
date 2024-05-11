namespace GeoMeApp.Services
{
    public interface ILocationService
    {
        double LocationUpdateSeconds { get; set; }
        Location? GetLocation();
        void StartLocationUpdates();
        void StopLocationUpdates();
        void OnSleep();
        void OnResume();
    }
}
