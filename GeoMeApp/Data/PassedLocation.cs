using System.ComponentModel.DataAnnotations;

namespace GeoMeApp.Data
{
    internal class PassedLocation
    {
        [Key]
        public int Id { get; set; }
        public double Latitude {  get; set; }
        public double Longitude { get; set; }
        public DateTime Time { get; set; }
    }
}
