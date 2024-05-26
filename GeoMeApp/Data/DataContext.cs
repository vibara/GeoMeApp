using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace GeoMeApp.Data
{
    internal class DataContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; } = null!;
        public DbSet<PassedLocation> PassedLocations { get; set; } = null!;
        private string _dataFilePath;

        public DataContext(string dataFilePath)
        {
            _dataFilePath = dataFilePath;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            


        }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString);
        }

        private string ConnectionString
        {
            get { return $"Data Source={_dataFilePath}"; }
        }
    }
}
