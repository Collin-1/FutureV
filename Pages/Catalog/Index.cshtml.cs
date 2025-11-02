using FutureV.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FutureV.Pages.Catalog;

public class IndexModel(ApplicationDbContext context) : PageModel
{
    private readonly ApplicationDbContext _context = context;

    public IReadOnlyList<CatalogItem> Cars { get; private set; } = Array.Empty<CatalogItem>();

    public async Task OnGetAsync()
    {
        var cars = await _context.Cars
            .Include(car => car.Images)
            .OrderBy(car => car.Name)
            .ToListAsync();

        Cars = cars
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
                car.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.GetImageSource())
                    .FirstOrDefault()))
            .ToList();
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
