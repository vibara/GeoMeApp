using GeoMeApp.Services;
using Microsoft.Maui.Maps;

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

        public MainPage()
        {
            InitializeComponent();
            _locationService = _app.Handler.MauiContext?.Services.GetService<ILocationService>();
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
    }

}
