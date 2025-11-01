using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FutureV.Pages.Admin;

public class LogoutModel(AdminAccessService adminAccessService) : PageModel
{
    private readonly AdminAccessService _adminAccessService = adminAccessService;

    [TempData]
    public string? StatusMessage { get; set; }

    public IActionResult OnPost()
    {
        _adminAccessService.RevokeAccess(HttpContext);
        StatusMessage = "Admin session cleared.";
        return RedirectToPage("/Admin/Login");
    }
}
