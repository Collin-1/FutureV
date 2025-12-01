using FutureV.Data;
using FutureV.Models;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FutureV.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context) => _context = context;

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        var model = await _context.Cars
            .AsNoTracking()
            .OrderByDescending(car => car.ReleaseYear)
            .Select(car => new CarSummary(
                car.Id,
                car.Name,
                car.Tagline,
                car.BasePrice,
                car.AutonomyLevel,
                car.RangePerCharge,
                car.TopSpeed,
                car.ZeroToSixty,
                car.Images.OrderBy(image => image.Id).Select(image => image.ImageUrl).FirstOrDefault()))
            .ToListAsync();

        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Contact(string? car)
    {
        var model = new ContactInputModel();
        if (!string.IsNullOrEmpty(car))
        {
            model.VehicleInterest = car;
        }
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Contact(ContactInputModel input)
    {
        if (!ModelState.IsValid)
        {
            return View(input);
        }

        TempData["StatusMessage"] = "Thanks for reaching out. A FutureV curator will respond shortly.";
        return RedirectToAction(nameof(Contact), new { car = input.VehicleInterest });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public record CarSummary(
        int Id,
        string Name,
        string Tagline,
        decimal BasePrice,
        string Autonomy,
        int Range,
        int TopSpeed,
        double ZeroToSixty,
        string? HeroImageSrc
    );
}
