using FutureV.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FutureV.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context) => _context = context;

    public IReadOnlyList<CarSummary> Cars { get; private set; } = Array.Empty<CarSummary>();

    public async Task OnGetAsync()
    {
        var cars = await _context.Cars
            .Include(car => car.Images)
            .OrderByDescending(car => car.ReleaseYear)
            .ToListAsync();

        Cars = cars
            .Select(car => new CarSummary(
                car.Id,
                car.Name,
                car.Tagline,
                car.BasePrice,
                car.AutonomyLevel,
                car.RangePerCharge,
                car.TopSpeed,
                car.ZeroToSixty,
                car.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.GetImageSource())
                    .FirstOrDefault()))
            .ToList();
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
