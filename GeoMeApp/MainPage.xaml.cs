namespace GeoMeApp
{
    public partial class MainPage : ContentPage
    {
        public double ControlUpdateSeconds { get; private set; } = 5;
        const string NoInfoAboutLocation = "...";
        private App _app = Application.Current as App;
        private Timer _updateTimer;
        public MainPage()
        {
            InitializeComponent();

            StartUpdateTimer();
        }

        void StartUpdateTimer()
        {
            // Initialize and start a timer to update the label every second
            _updateTimer = new Timer(UpdateControls, null, TimeSpan.Zero, TimeSpan.FromSeconds(ControlUpdateSeconds));
        }

        void UpdateControls(object state)
        {
            // Update the label text with the current time
            MainThread.BeginInvokeOnMainThread(() =>
            {
                var location = _app.Location;
                if (location != null) {
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
