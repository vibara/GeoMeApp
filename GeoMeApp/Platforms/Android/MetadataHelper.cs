using Android.App;
using Android.Content.PM;

namespace GeoMeApp
{
    public static class MetadataHelper
    {
        public static void SetMetaDataValue(string key, string value)
        {
            try
            {
                if (Android.App.Application.Context.PackageName != null)
                {
                    ApplicationInfo? appInfo = Android.App.Application.Context.PackageManager?.GetApplicationInfo(Android.App.Application.Context.PackageName, PackageInfoFlags.MetaData);
                    appInfo?.MetaData?.PutString(key, value);
                }
            }
            catch
            {
                // Handle exception (e.g., key not found)
                
            }
        }
    }
}
