using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

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

        foreach (var image in Car.Images)
        {
            if (!image.HasAnySource())
            {
                continue;
            }

            var galleryImage = new CarImage
            {
                ViewAngle = image.ViewAngle,
                Description = image.Description
            };

            if (image.ImageFile is { Length: > 0 })
            {
                using var memoryStream = new MemoryStream();
                await image.ImageFile.CopyToAsync(memoryStream);
                galleryImage.ImageData = memoryStream.ToArray();
                galleryImage.ContentType = image.ImageFile.ContentType;
                galleryImage.FileName = Path.GetFileName(image.ImageFile.FileName);
                galleryImage.ImageUrl = null;
            }
            else if (!string.IsNullOrWhiteSpace(image.ImageUrl))
            {
                galleryImage.ImageUrl = image.ImageUrl;
                galleryImage.ImageData = null;
                galleryImage.ContentType = null;
                galleryImage.FileName = null;
            }

            entity.Images.Add(galleryImage);
        }

        if (entity.Images.Count == 0)
        {
            ModelState.AddModelError("Car.Images", "Add at least one image for the vehicle gallery.");
            await transaction.RollbackAsync();
            return Page();
        }

        _context.Cars.Add(entity);
        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        TempData["StatusMessage"] = $"Added {entity.Name} to the FutureV catalog.";
        return RedirectToPage("Index");
    }
}
