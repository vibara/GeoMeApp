using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services
{
    public class SettingsService : ISettingsService
    {
        public Task<T> GetValue<T>(string key, T defaultValue)
        {
            var result = Preferences.Default.Get(key, defaultValue);
            return Task.FromResult(result);
        }
        public Task SetValue<T>(string key, T value)
        {
            Preferences.Default.Set(key, value);
            return Task.CompletedTask;
        }
    }
}
