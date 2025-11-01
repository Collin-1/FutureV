using System.ComponentModel.DataAnnotations;

namespace FutureV.Models;

public class CarImage
{
    public int Id { get; set; }

    [Required]
    public int CarId { get; set; }

    public Car? Car { get; set; }

    [Required, StringLength(200)]
    public string ViewAngle { get; set; } = string.Empty;

    [Required, StringLength(500)]
    public string ImageUrl { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; set; }
}
