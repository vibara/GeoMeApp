using System.ComponentModel.DataAnnotations;

namespace GeoMeApp.Data;

public class PassedPath
{
    [Key]
    public int Id { get; set; }
    public ICollection<PassedLocation> PassedLocations { get; set; } = null!;
}
