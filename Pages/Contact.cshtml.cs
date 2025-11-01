using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FutureV.Pages;

public class ContactModel : PageModel
{
    [BindProperty]
    public ContactInputModel Input { get; set; } = new();

    [TempData]
    public string? StatusMessage { get; set; }

    public void OnGet(string? car)
    {
        if (!string.IsNullOrEmpty(car))
        {
            Input.VehicleInterest = car;
        }
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        StatusMessage = "Thanks for reaching out. A FutureV curator will respond shortly.";
        return RedirectToPage(new { car = Input.VehicleInterest });
    }

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
}
