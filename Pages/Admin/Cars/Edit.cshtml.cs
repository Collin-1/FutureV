using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Admin.Cars;

public class EditModel : AdminPageModel
{
    private readonly ApplicationDbContext _context;

    public EditModel(ApplicationDbContext context, AdminAccessService adminAccessService) : base(adminAccessService)
    {
        _context = context;
    }

    [BindProperty]
    public CarInputModel Car { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var entity = await _context.Cars
            .Include(car => car.Images)
            .FirstOrDefaultAsync(car => car.Id == Id);

        if (entity is null)
        {
            return NotFound();
        }

        Car = new CarInputModel
        {
            Name = entity.Name,
            Tagline = entity.Tagline,
            BasePrice = entity.BasePrice,
            DriveType = entity.DriveType,
            EnergySystem = entity.EnergySystem,
            AutonomyLevel = entity.AutonomyLevel,
            PowerOutput = entity.PowerOutput,
            RangePerCharge = entity.RangePerCharge,
            TopSpeed = entity.TopSpeed,
            ZeroToSixty = entity.ZeroToSixty,
            FastChargeMinutes = entity.FastChargeMinutes,
            SeatingCapacity = entity.SeatingCapacity,
            ReleaseYear = entity.ReleaseYear,
            Narrative = entity.Narrative,
            Images = entity.Images
                .OrderBy(image => image.Id)
                .Select(image => new CarImageInputModel
                {
                    ViewAngle = image.ViewAngle,
                    ImageUrl = image.ImageUrl,
                    Description = image.Description
                })
                .ToList()
        };

        if (Car.Images.Count == 0)
        {
            Car.Images.Add(new CarImageInputModel { ViewAngle = "Front" });
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        var entity = await _context.Cars
            .Include(car => car.Images)
            .FirstOrDefaultAsync(car => car.Id == Id);

        if (entity is null)
        {
            return NotFound();
        }

        entity.Name = Car.Name;
        entity.Tagline = Car.Tagline;
        entity.BasePrice = Car.BasePrice;
        entity.DriveType = Car.DriveType;
        entity.EnergySystem = Car.EnergySystem;
        entity.AutonomyLevel = Car.AutonomyLevel;
        entity.PowerOutput = Car.PowerOutput;
        entity.RangePerCharge = Car.RangePerCharge;
        entity.TopSpeed = Car.TopSpeed;
        entity.ZeroToSixty = Car.ZeroToSixty;
        entity.FastChargeMinutes = Car.FastChargeMinutes;
        entity.SeatingCapacity = Car.SeatingCapacity;
        entity.ReleaseYear = Car.ReleaseYear;
        entity.Narrative = Car.Narrative;

        var updatedImages = Car.Images
            .Where(image => !string.IsNullOrWhiteSpace(image.ImageUrl))
            .Select(image => new CarImage
            {
                ViewAngle = image.ViewAngle,
                ImageUrl = image.ImageUrl,
                Description = image.Description
            })
            .ToList();

        entity.Images.Clear();
        foreach (var image in updatedImages)
        {
            entity.Images.Add(image);
        }

        await _context.SaveChangesAsync();
        TempData["StatusMessage"] = $"Updated {entity.Name}.";
        return RedirectToPage("Index");
    }
}
