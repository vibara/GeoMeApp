using GeoMeApp.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;

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

        private Polyline _myTrack = new Polyline   // To refactor
        {
            StrokeColor = Color.FromArgb("#FF0000"),
            StrokeWidth = 5
        };

        public MainPage()
        {
            InitializeComponent();
            _locationService = _app?.Handler.MauiContext?.Services.GetService<ILocationService>();
            _databaseService = _app?.Handler.MauiContext?.Services.GetService<IDatabaseService>();
            Map.IsVisible = false;
            if (_databaseService != null ) {
                var locations = _databaseService.GetLocations();
                foreach (var location in locations )
                {
                    _myTrack.Geopath.Add(location);
                }
                // To refactor. ((List<Location>)_myTrack.Geopath).AddRange(locations); ? What's wrong?
            }
            StartUpdateTimer();
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
                        Map.IsVisible = true;
                        Map.MapElements.Add(_myTrack);
                    }
                    if (!_initialCentering && _polylineDrawing)
                    {
                        if (_myTrack.Geopath.Count == 0 || 
                            _myTrack.Geopath.Last().Latitude != location.Latitude || 
                            _myTrack.Geopath.Last().Longitude != location.Longitude)
                        {
                            _myTrack.Geopath.Add(location);
                            _databaseService?.AddLocation(location, DateTime.Now);   
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
            _polylineDrawing = e.Value;
        }
    }

}
