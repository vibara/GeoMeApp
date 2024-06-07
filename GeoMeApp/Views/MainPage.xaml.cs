using GeoMeApp.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;
using GeoMeApp.Data;

namespace GeoMeApp.Views
{
    public partial class MainPage : ContentPage
    {
        public double ControlUpdateSeconds { get; private set; } = 10;
        private const string NoInfoAboutLocation = "...";
        private readonly App? _app = Application.Current as App;
        private readonly ILocationService? _locationService;
        private Timer? _updateTimer;
        private bool _initialCentering = true;
        private bool _polylineDrawing = false;
        private readonly IDatabaseService? _databaseService;
        private PassedPath? _currentPath = null;
        private Polyline? _currentTrack = null;

        public MainPage()
        {
            InitializeComponent();
            _locationService = _app?.Handler.MauiContext?.Services.GetService<ILocationService>();
            _databaseService = _app?.Handler.MauiContext?.Services.GetService<IDatabaseService>();
            RestoreDataFromDatabase();
            StartUpdateTimer();
        }

        private static Polyline CreateGeopath()
        {
            var geopath = new Polyline()
            {
                StrokeColor = Color.FromArgb("#FF0000"),
                StrokeWidth = 3
            };
            return geopath;
        }

        private void NewGeopath()
        { 
            _currentTrack = CreateGeopath();
            Map.MapElements.Add(_currentTrack);
            if (_databaseService != null)
            {
                _currentPath = _databaseService.AddPath();
            }
        }

        private void RestoreDataFromDatabase()
        {
            if (_databaseService != null)
            {
                foreach (var path in _databaseService.GetPaths())
                {
                    var geopath = CreateGeopath();
                    var locations = _databaseService.GetLocations(path);
                    foreach (var location in locations)
                    {
                        geopath.Add(location);
                    }
                    Map.MapElements.Add(geopath);
                }
                // To refactor. ((List<Location>)geopPath).AddRange(locations); ? What's wrong?
            }
        }

        private void StartUpdateTimer()
        {
            _updateTimer = new Timer(UpdateControls, null, TimeSpan.Zero, TimeSpan.FromSeconds(ControlUpdateSeconds));
        }

        private void UpdateControls(object? state)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var location = _locationService?.GetLocation();
                if (location != null)
                {
                    Coordinates.Text = $"Lat: {location.Latitude:F4}  Long: {location.Longitude:F4}";
                    if (_initialCentering)
                    {
                        MapSpan mapSpan = MapSpan.FromCenterAndRadius(location, Distance.FromKilometers(0.444));
                        Map.MoveToRegion(mapSpan);
                        _initialCentering = false;
                        if (_polylineDrawing)
                        {
                            NewGeopath();
                        }
                    }
                    if (!_initialCentering && _polylineDrawing)
                    {
                        if (_currentTrack == null)
                        {
                            NewGeopath();
                        }
                        if (_currentTrack != null && (
                            _currentTrack.Count == 0 ||
                            _currentTrack.Last().Latitude != location.Latitude ||
                            _currentTrack.Last().Longitude != location.Longitude))
                        {
                            _currentTrack.Add(location);
                            _databaseService?.AddLocation(_currentPath, location, DateTime.Now);   
                        }
                    }
                }
                else
                {
                    Coordinates.Text = NoInfoAboutLocation;
                }
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _updateTimer?.Dispose();
        }

        private void Track_Toggled(object sender, ToggledEventArgs e)
        {
            if (!_polylineDrawing && e.Value)
            {
                NewGeopath();
            }
            _polylineDrawing = e.Value;
            if (!_polylineDrawing)
            {
                _currentPath = null;
                _currentTrack = null;
            }
        }
    }

}
