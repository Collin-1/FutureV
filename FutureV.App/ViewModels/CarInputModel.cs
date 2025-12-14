using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FutureV.ViewModels;

public class CarInputModel
{
    [Display(Name = "Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Tagline")]
    public string Tagline { get; set; } = string.Empty;

    [Display(Name = "Base Price (credits)")]
    public decimal BasePrice { get; set; }

    [Display(Name = "Drive Type")]
    public string DriveType { get; set; } = string.Empty;

    [Display(Name = "Energy System")]
    public string EnergySystem { get; set; } = string.Empty;

    [Display(Name = "Autonomy Level")]
    public string AutonomyLevel { get; set; } = string.Empty;

    [Display(Name = "Power Output")]
    public string PowerOutput { get; set; } = string.Empty;

    [Display(Name = "Range per Charge (km)")]
    public int RangePerCharge { get; set; }

    [Display(Name = "Top Speed (km/h)")]
    public int TopSpeed { get; set; }

    [Display(Name = "0-60 mph (seconds)")]
    public double ZeroToSixty { get; set; }

    [Display(Name = "Fast Charge (minutes)")]
    public double FastChargeMinutes { get; set; }

    [Display(Name = "Seating Capacity")]
    public int SeatingCapacity { get; set; }

    [Display(Name = "Release Year")]
    public int ReleaseYear { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Narrative")]
    public string Narrative { get; set; } = string.Empty;

    public List<CarImageInputModel> Images { get; set; } = new();
}

public class CarImageInputModel : IValidatableObject
{
    public int? Id { get; set; }

    [Required, StringLength(200)]
    [Display(Name = "View Angle")]
    public string ViewAngle { get; set; } = string.Empty;

    [StringLength(500)]
    [Url]
    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Display(Name = "Upload Image")]
    public IFormFile? ImageFile { get; set; }

    [StringLength(200)]
    [Display(Name = "Description")]
    public string? Description { get; set; }

    public bool HasStoredImage { get; set; }

    public string? ExistingImageSource { get; set; }

    public bool HasAnySource() =>
        (ImageFile is { Length: > 0 }) ||
        !string.IsNullOrWhiteSpace(ImageUrl) ||
        (HasStoredImage && Id.HasValue);

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (HasAnySource())
        {
            yield break;
        }

        yield return new ValidationResult(
            "Provide a URL or upload a file for this gallery image.",
            new[] { nameof(ImageUrl), nameof(ImageFile) });
    }
}
