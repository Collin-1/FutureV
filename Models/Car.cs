using System.ComponentModel.DataAnnotations;

namespace FutureV.Models;

public class Car
{
    public int Id { get; set; }

    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Tagline { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string DriveType { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string EnergySystem { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string AutonomyLevel { get; set; } = string.Empty;

    [Required, StringLength(80)]
    public string PowerOutput { get; set; } = string.Empty;

    [Required, Range(0, 10_000_000)]
    public decimal BasePrice { get; set; }

    [Required, Range(0, 1000)]
    public int RangePerCharge { get; set; }

    [Required, Range(0, 500)]
    public int TopSpeed { get; set; }

    [Required, Range(0, 10)]
    public double ZeroToSixty { get; set; }

    [Required, Range(0, 24)]
    public double FastChargeMinutes { get; set; }

    [Required, Range(1, 12)]
    public int SeatingCapacity { get; set; }

    [Required, Range(2024, 2100)]
    public int ReleaseYear { get; set; }

    [DataType(DataType.MultilineText)]
    [StringLength(2000)]
    public string Narrative { get; set; } = string.Empty;

    public ICollection<CarImage> Images { get; set; } = new List<CarImage>();
}
