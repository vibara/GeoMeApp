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

    public void AddLocation(PassedPath? path, Location location, DateTime when)
    {
        if (path == null)
        {
            return;
        }
        using var dataContext = GetDataContext();  
        if (dataContext != null)
        {
            var passedLocation = new PassedLocation() { 
                Latitude = location.Latitude, 
                Longitude = location.Longitude,  
                Time = when,
                PathId = path.Id
            };
            dataContext.PassedLocations.Add(passedLocation);
            dataContext.SaveChanges();
        }
    }

    public PassedPath? AddPath()
    {
        using var dataContext = GetDataContext();
        if (dataContext != null)
        {
            var path = dataContext.PassedPaths.Add(new PassedPath() { });
            dataContext.SaveChanges();
            return path.Entity;
        }
        else
        {
            return null;
        }
    }

    public IList<Location> GetLocations(PassedPath path)
    {
        List<Location> locations = new List<Location>();
        using var dataContext = GetDataContext();
        if (dataContext != null)
        {
            dataContext.PassedLocations
                .Where(l => l.PassedPath.Id == path.Id)
                .ToList()
                .ForEach(location => locations.Add(new Location()
            {
                Latitude = location.Latitude,
                Longitude = location.Longitude
            }
            ));
        }
        return locations;
    }

    public IList<PassedPath> GetPaths()
    {
        List<PassedPath> paths = new List<PassedPath>();
        using var dataContext = GetDataContext();
        if (dataContext != null)
        {
            paths = dataContext.PassedPaths.ToList();
        }
        return paths;
    }
}

