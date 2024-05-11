using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace GeoMeApp.Data
{
    internal class Trip
    {
        [Key]
        public int Id { get; set; }
        public ICollection<PassedLocation>? PassedLocations { get; set; } = null;
    }
}
