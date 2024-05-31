using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoMeApp.Data;
using Microsoft.Maui.Devices.Sensors;

namespace GeoMeApp.Services;

public interface IDatabaseService
{
    public DataContext GetDataContext(bool initialize = false);
    public void AddLocation(Location location, DateTime when);
    public IList<Location> GetLocations();
}
