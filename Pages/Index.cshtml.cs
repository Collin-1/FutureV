using FutureV.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages;

public class IndexModel : PageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context) => _context = context;

    public IReadOnlyList<CarSummary> Cars { get; private set; } = Array.Empty<CarSummary>();

    public async Task OnGetAsync()
    {
        Cars = await _context.Cars
            .Include(car => car.Images)
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
                car.Images
                    .OrderBy(image => image.Id)
                    .Select(image => image.ImageUrl)
                    .FirstOrDefault()))
            .ToListAsync();
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
        string? HeroImageUrl
    );
}
