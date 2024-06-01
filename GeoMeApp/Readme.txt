

In order to build a project, you need to add the file "appsettings.json" to the main folder of the project.

The file should contain the following structure:

{
  "AppSettings": {
    "BingMaps_Key": "BINGMAPS_KEY",
    "Android_GeoApi_Key": "ANDROID_MAPS_KEY"
  }
}

where:
BINGMAPS_KEY - the key for Bing Maps (maps for Windows). You can create there: https://www.bingmapsportal.com/Application
ANDROID_MAPS_KEY - the key for Android (Google) Maps. You can create it there: https://console.cloud.google.com/projectselector2/google/maps-apis/credentials?utm_source=Docs_CreateAPIKey&utm_content=Docs_maps-backend
(documentation is there: https://developers.google.com/maps/documentation/javascript/get-api-key)