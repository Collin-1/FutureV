using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FutureV.ViewModels;

public class CarInputModel : IValidatableObject
{
    [Required, StringLength(80)]
    public string Name { get; set; } = string.Empty;

    [Required, StringLength(120)]
    public string Tagline { get; set; } = string.Empty;

    [Required, Range(0, 10_000_000)]
    [Display(Name = "Base Price (credits)")]
    public decimal BasePrice { get; set; }

    [Required, StringLength(80)]
    [Display(Name = "Drive Type")]
    public string DriveType { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Energy System")]
    public string EnergySystem { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Autonomy Level")]
    public string AutonomyLevel { get; set; } = string.Empty;

    [Required, StringLength(80)]
    [Display(Name = "Power Output")]
    public string PowerOutput { get; set; } = string.Empty;

    [Required, Range(0, 1500)]
    [Display(Name = "Range per Charge (km)")]
    public int RangePerCharge { get; set; }

    [Required, Range(0, 500)]
    [Display(Name = "Top Speed (km/h)")]
    public int TopSpeed { get; set; }

    [Required, Range(0, 10)]
    [Display(Name = "0-60 mph (seconds)")]
    public double ZeroToSixty { get; set; }

    [Required, Range(0, 24)]
    [Display(Name = "Fast Charge (minutes)")]
    public double FastChargeMinutes { get; set; }

    [Required, Range(1, 12)]
    [Display(Name = "Seating Capacity")]
    public int SeatingCapacity { get; set; }

    [Required, Range(2024, 2100)]
    [Display(Name = "Release Year")]
    public int ReleaseYear { get; set; }

    [StringLength(2000)]
    public string Narrative { get; set; } = string.Empty;

    public List<CarImageInputModel> Images { get; set; } = new();

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (Images.Count == 0)
        {
            yield return new ValidationResult("Add at least one image for the vehicle gallery.", new[] { nameof(Images) });
            yield break;
        }

        for (var index = 0; index < Images.Count; index++)
        {
            if (Images[index].HasAnySource())
            {
                continue;
            }

            yield return new ValidationResult(
                "Provide a URL or upload a file for each gallery image.",
                new[] { $"Images[{index}].ImageUrl", $"Images[{index}].ImageFile" });
        }
    }
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
