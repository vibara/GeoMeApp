namespace GeoMeApp;

public partial class App : Application
{
    public Location? Location { get; private set; }
    public double LocationUpdateSeconds { get; private set; } = 5;
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private Timer _locationUpdateTimer;

    public string DataFilePath { get; private set; }
    private const string DataFileName = "GeoMeData.db";


    public App()
    {
        InitializeComponent();
        DataFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, DataFileName);

        MainPage = new AppShell();
        StartLocationUpdates();
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
        _locationUpdateTimer = new Timer(RequestLocation, null, TimeSpan.Zero, TimeSpan.FromSeconds(LocationUpdateSeconds)); 
    }

    protected override void OnSleep()
    {
        _locationUpdateTimer?.Dispose();
        _locationUpdateTimer = null;
    }
    protected override void OnResume()
    {
        StartLocationUpdates();
    }
}
