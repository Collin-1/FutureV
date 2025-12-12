using FutureV.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Controllers;

public class CatalogController : Controller
{
    private readonly ApplicationDbContext _context;

    public CatalogController(ApplicationDbContext context)
    {
        _context = context;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        var model = await _context.Cars
            .AsNoTracking()
            .OrderBy(car => car.Name)
            .Select(car => new CatalogItem(
                car.Id,
                car.Name,
                car.Tagline,
                car.BasePrice,
                car.AutonomyLevel,
                car.DriveType,
                car.EnergySystem,
                car.RangePerCharge,
                car.TopSpeed,
                car.ZeroToSixty,
                car.Images.OrderBy(image => image.Id).Select(image => image.ImageUrl).FirstOrDefault()))
            .ToListAsync();

        return View(model);
    }

    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]
    public async Task<IActionResult> Details(int id)
    {
        var car = await _context.Cars
            .AsNoTracking()
            .Include(car => car.Images)
            .FirstOrDefaultAsync(car => car.Id == id);

        if (car is null)
        {
            return NotFound();
        }

        return View(car);
    }

    public record CatalogItem(
        int Id,
        string Name,
        string Tagline,
        decimal BasePrice,
        string Autonomy,
        string DriveType,
        string EnergySystem,
        int Range,
        int TopSpeed,
        double ZeroToSixty,
        string? HeroImageSrc
    );
}
