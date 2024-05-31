using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoMeApp.Services;

public interface ISettingsService
{
    Task<T> GetValue<T>(string key, T defaultValue);
    Task SetValue<T>(string key, T value);
}
