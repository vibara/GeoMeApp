using GeoMeApp.Services;

namespace GeoMeApp;

public partial class App : Application
{
    public string DataFilePath { get; private set; }
    private const string DataFileName = "GeoMeData.db";
    public IServiceProvider Services { get; private set; }


    public App(IServiceProvider provider)
    {
        InitializeComponent();
        DataFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, DataFileName);
        Services = provider;
        MainPage = new AppShell();
        var _locationService = Services.GetService<ILocationService>();
        if (_locationService != null )
        {
            _locationService.StartLocationUpdates();
        }
    }
}
