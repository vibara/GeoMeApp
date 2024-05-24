using GeoMeApp.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Diagnostics;

namespace GeoMeApp
{
    public partial class MainPage : ContentPage
    {
        public double ControlUpdateSeconds { get; private set; } = 5;
        private const string NoInfoAboutLocation = "...";
        private readonly App _app = Application.Current as App;
        private readonly ILocationService? _locationService;
        private Timer? _updateTimer;
        private bool _initialCentering = true;
        private bool _polylineDrawing = false;
        private Polyline _myTrack = new Polyline   // To refactor
        {
            StrokeColor = Color.FromArgb("#FF0000"),
            StrokeWidth = 5
        };

        public MainPage()
        {
            InitializeComponent();
            _locationService = _app.Handler.MauiContext?.Services.GetService<ILocationService>();
            Map.MapElements.Add(_myTrack);
            StartUpdateTimer();
        }

        void StartUpdateTimer()
        {
            _updateTimer = new Timer(UpdateControls, null, TimeSpan.Zero, TimeSpan.FromSeconds(ControlUpdateSeconds));
        }

        void UpdateControls(object state)
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
                    }
                    if (!_initialCentering && _polylineDrawing)
                    {
                        Map.MapElements.Clear();
                        if (_myTrack.Count == 0 || (_myTrack.First().Latitude != location.Latitude || _myTrack.First().Longitude != location.Longitude)) 
                        {
                            _myTrack.Add(location);
                        }
                        Map.MapElements.Add(_myTrack);
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
            if (e.Value)
            {
                _polylineDrawing = true;
            }
            else
            {
                _polylineDrawing = false;
                Map.MapElements.Clear();
                _myTrack.Clear();
            }
        }
    }

}
