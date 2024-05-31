using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GeoMeApp.Data;

public class DataContext : DbContext
{
    public DbSet<PassedLocation> PassedLocations { get; set; } = null!;
    private string _dataFilePath;

    private string ConnectionString
    {
        get { return $"Data Source={_dataFilePath}"; }
    }

    public DataContext(string dataFilePath)
    {
        _dataFilePath = dataFilePath;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PassedLocation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Time).IsRequired();
            entity.Property(e => e.Latitude).IsRequired();
            entity.Property(e => e.Longitude).IsRequired();
        }); 
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);
    }
}
