using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FutureV.Controllers;

public class AdminController : Controller
{
    private readonly AdminAccessService _adminAccessService;

    public AdminController(AdminAccessService adminAccessService)
    {
        _adminAccessService = adminAccessService;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl)
    {
        if (_adminAccessService.HasAccess(HttpContext))
        {
            return RedirectToAction("Index", "Cars");
        }

        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginInputModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginInputModel input, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View(input);
        }

        if (!_adminAccessService.ValidatePassphrase(input.Passphrase))
        {
            TempData["ErrorMessage"] = "Incorrect passphrase. Hint: keep it thin.";
            return RedirectToAction(nameof(Login), new { returnUrl });
        }

        _adminAccessService.GrantAccess(HttpContext);

        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }

        return RedirectToAction("Index", "Cars");
    }

    public IActionResult Logout()
    {
        _adminAccessService.RevokeAccess(HttpContext);
        return RedirectToAction("Index", "Home");
    }

    public class LoginInputModel
    {
        [Display(Name = "Admin Passphrase")]
        [Required(ErrorMessage = "Enter the admin passphrase.")]
        public string Passphrase { get; set; } = string.Empty;
    }
}
