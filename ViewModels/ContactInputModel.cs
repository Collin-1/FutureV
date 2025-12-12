using System.ComponentModel.DataAnnotations;

namespace FutureV.ViewModels;

public class ContactInputModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Organization or Collective")]
    public string? Organization { get; set; }

    [Display(Name = "Vehicle of Interest")]
    public string? VehicleInterest { get; set; }

    [Required]
    public string Message { get; set; } = string.Empty;
}
