namespace GeoMeApp.Services;

public interface ILocationService
{
    double LocationUpdateSeconds { get; set; }
    Location? GetLocation();
}
