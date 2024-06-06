using System.ComponentModel.DataAnnotations;

namespace GeoMeApp.Data;

public class PassedLocation
{
    [Key]
    public int Id { get; set; }
    public double Latitude {  get; set; }
    public double Longitude { get; set; }
    public DateTime Time { get; set; }
    public int PathId { get; set; }
    public PassedPath PassedPath { get; set; } = null!; 
}
