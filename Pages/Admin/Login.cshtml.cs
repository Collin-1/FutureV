using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FutureV.Pages.Admin;

public class LoginModel(AdminAccessService adminAccessService) : PageModel
{
    private readonly AdminAccessService _adminAccessService = adminAccessService;

    [BindProperty]
    [Display(Name = "Admin Passphrase")]
    [Required(ErrorMessage = "Enter the admin passphrase.")]
    public string Passphrase { get; set; } = string.Empty;

    [TempData]
    public string? ErrorMessage { get; set; }

    [TempData]
    public string? StatusMessage { get; set; }

    [FromQuery]
    public string? ReturnUrl { get; set; }

    public IActionResult OnGet()
    {
        if (_adminAccessService.HasAccess(HttpContext))
        {
            return RedirectToPage("/Admin/Cars/Index");
        }

        return Page();
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!_adminAccessService.ValidatePassphrase(Passphrase))
        {
            ErrorMessage = "Incorrect passphrase. Hint: keep it thin.";
            return RedirectToPage(new { ReturnUrl });
        }

        _adminAccessService.GrantAccess(HttpContext);

        if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
        {
            return Redirect(ReturnUrl);
        }

        return RedirectToPage("/Admin/Cars/Index");
    }
}
