namespace GeoMeApp
{
    public partial class App : Application
    {
        public string DataFilePath { get; private set; }
        private const string DataFileName = "GeoMeData.db";

        public App()
        {
            InitializeComponent();
            DataFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, DataFileName);

            MainPage = new AppShell();
        }
    }
}
