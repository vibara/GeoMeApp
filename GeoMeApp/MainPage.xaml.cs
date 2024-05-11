using GeoMeApp.Services;

namespace GeoMeApp
{
    public partial class MainPage : ContentPage
    {
        public double ControlUpdateSeconds { get; private set; } = 5;
        const string NoInfoAboutLocation = "...";
        private App _app = Application.Current as App;
        private ILocationService? _locationService;
        private Timer? _updateTimer;

        public MainPage()
        {
            InitializeComponent();
            _locationService = Application.Current.Handler.MauiContext?.Services.GetService<ILocationService>();
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
                    LatValue.Text = $"{location.Latitude:F3}";
                    LongValue.Text = $"{location.Longitude:F3}";
                }
                else
                {
                    LatValue.Text = NoInfoAboutLocation;
                    LongValue.Text = NoInfoAboutLocation;
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
