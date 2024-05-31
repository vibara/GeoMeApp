using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeoMeApp.Data;
using Microsoft.Maui.Devices.Sensors;

namespace GeoMeApp.Services;

public class DatabaseService : Data.DataContext, IDatabaseService
{
    private string _dataFilePath = string.Empty;

    public DatabaseService(string dataFilePath) : base(dataFilePath) 
    {
        Debug.Assert(dataFilePath != null && dataFilePath.Length > 0);
        _dataFilePath = dataFilePath;
    }

    public DataContext GetDataContext(bool initialize = false)
    {
        var dataContext = new DataContext(_dataFilePath);
        if (initialize)
        {
            dataContext.Database.EnsureCreated();
        }
        return dataContext;

    }

    public void AddLocation(Location location, DateTime when)
    {
        using var dataContext = GetDataContext();  
        if (dataContext != null)
        {
            var passedLocation = new PassedLocation() { 
                Latitude = location.Latitude, 
                Longitude = location.Longitude,  
                Time = when
            };
            dataContext.PassedLocations.Add(passedLocation);
            dataContext.SaveChanges();
        }
    }

    public IList<Location> GetLocations()
    {
        List<Location> locations = new List<Location>();
        using var dataContext = GetDataContext();
        if (dataContext != null)
        {
            dataContext.PassedLocations.ToList().ForEach(location => locations.Add(new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }
            ));
        }
        return locations;
    }
}

