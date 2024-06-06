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
        modelBuilder.Entity<PassedLocation>(location =>
        {
            location.HasKey(e => e.Id);
            location.Property(e => e.Time).IsRequired();
            location.Property(e => e.Latitude).IsRequired();
            location.Property(e => e.Longitude).IsRequired();
        });

        modelBuilder.Entity<PassedPath>(path =>
        {
            path.HasKey(e => e.Id);
        });

        modelBuilder.Entity<PassedPath>()
            .HasMany(e => e.PassedLocations)
            .WithOne(e => e.PassedPath)
            .HasForeignKey(e => e.PathId);
    }
        
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(ConnectionString);
    }
}
