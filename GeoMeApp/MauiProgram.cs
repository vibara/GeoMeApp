﻿using GeoMeApp.Services;
using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Maps;
using Microsoft.Extensions.Configuration;
using System.Reflection;


namespace GeoMeApp
{
    public static class MauiProgram
    {
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

            /* adding appsettings.json with passwords */
            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("GeoMeApp.appsettings.json");
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


#if DEBUG
            builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ISettingsService, SettingsService>();
            builder.Services.AddSingleton<ILocationService, LocationService>();
            return builder.Build();
        }
    }
}
