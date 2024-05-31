using GeoMeApp.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Maps;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace GeoMeApp
{
    public static class MauiProgram
    {

        private const string DataFileName = "GeoMeData.db";
        public static string DataFilePath { get; private set; } = Path.Combine(FileSystem.Current.AppDataDirectory, DataFileName); // example: GeoMeData "/data/user/0/com.companyname.geomeapp/files/GeoMeData.db"

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiMaps();
            AddConfiguration(builder);
#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            builder.Services.AddSingleton<IDatabaseService>(
                provider => new DatabaseService(DataFilePath) );
            return builder.Build();
        }

        private static void AddConfiguration(MauiAppBuilder builder)
        {
            /* adding appsettings.json with passwords */
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GeoMeApp.appsettings.json");
            if (stream != null)
            {
                IConfiguration configuration = new ConfigurationBuilder()
                    .AddJsonStream(stream)
                    .Build();
#if WINDOWS
                string keyMauiCommunityToolkit = configuration["AppSettings:BingMaps_Key"] ?? string.Empty;
#elif ANDROID
                string keyAndroidGeoApiKey = configuration["AppSettings:Android_GeoApi_Key"] ?? string.Empty;
                MetadataHelper.SetMetaDataValue("com.google.android.geo.API_KEY", keyAndroidGeoApiKey);
#endif
#if WINDOWS
                builder.UseMauiCommunityToolkitMaps(keyMauiCommunityToolkit); // https://learn.microsoft.com/en-us/bingmaps/getting-started/bing-maps-dev-center-help/getting-a-bing-maps-key
#endif
                builder.Configuration.AddConfiguration(configuration);
            }
        }


    }
}
