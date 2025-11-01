using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Admin.Cars;

public class CreateModel : AdminPageModel
{
    private readonly ApplicationDbContext _context;

    public CreateModel(ApplicationDbContext context, AdminAccessService adminAccessService) : base(adminAccessService)
    {
        Car = new CarInputModel
        {
            Images =
            [
                new CarImageInputModel { ViewAngle = "Front" },
                new CarImageInputModel { ViewAngle = "Side" },
                new CarImageInputModel { ViewAngle = "Rear" }
            ]
        };
        _context = context;
    }

    [BindProperty]
    public CarInputModel Car { get; set; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (!Car.Images.Any())
        {
            ModelState.AddModelError("Car.Images", "Add at least one image for the vehicle gallery.");
            return Page();
        }

        using var transaction = await _context.Database.BeginTransactionAsync();

        var entity = new Car
        {
            Name = Car.Name,
            Tagline = Car.Tagline,
            BasePrice = Car.BasePrice,
            DriveType = Car.DriveType,
            EnergySystem = Car.EnergySystem,
            AutonomyLevel = Car.AutonomyLevel,
            PowerOutput = Car.PowerOutput,
            RangePerCharge = Car.RangePerCharge,
            TopSpeed = Car.TopSpeed,
            ZeroToSixty = Car.ZeroToSixty,
            FastChargeMinutes = Car.FastChargeMinutes,
            SeatingCapacity = Car.SeatingCapacity,
            ReleaseYear = Car.ReleaseYear,
            Narrative = Car.Narrative
        };

        foreach (var image in Car.Images.Where(img => !string.IsNullOrWhiteSpace(img.ImageUrl)))
        {
            entity.Images.Add(new CarImage
            {
                ViewAngle = image.ViewAngle,
                ImageUrl = image.ImageUrl,
                Description = image.Description
            });
        }

        _context.Cars.Add(entity);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["StatusMessage"] = $"Added {entity.Name} to the FutureV catalog.";
        return RedirectToPage("Index");
    }
}
