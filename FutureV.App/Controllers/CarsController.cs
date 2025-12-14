using FutureV.Core.Entities;
using FutureV.Core.Interfaces;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FutureV.Controllers;

[Authorize]
public class CarsController : Controller
{
    private readonly ICarRepository _repository;

    public CarsController(ICarRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> Index()
    {
        var cars = await _repository.GetAllAsync();
        return View(cars);
    }

    [HttpGet]
    public IActionResult Create()
    {
        var model = new CarInputModel
        {
            Images =
            [
                new CarImageInputModel { ViewAngle = "Front" },
                new CarImageInputModel { ViewAngle = "Side" },
                new CarImageInputModel { ViewAngle = "Rear" }
            ]
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CarInputModel car)
    {
        if (!ModelState.IsValid)
        {
            return View(car);
        }

        var entity = new Car
        {
            Name = car.Name,
            Tagline = car.Tagline,
            BasePrice = car.BasePrice,
            DriveType = car.DriveType,
            EnergySystem = car.EnergySystem,
            AutonomyLevel = car.AutonomyLevel,
            PowerOutput = car.PowerOutput,
            RangePerCharge = car.RangePerCharge,
            TopSpeed = car.TopSpeed,
            ZeroToSixty = car.ZeroToSixty,
            FastChargeMinutes = car.FastChargeMinutes,
            SeatingCapacity = car.SeatingCapacity,
            ReleaseYear = car.ReleaseYear,
            Narrative = car.Narrative
        };

        foreach (var image in car.Images)
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
            ModelState.AddModelError("Images", "Add at least one image for the vehicle gallery.");
            return View(car);
        }

        await _repository.AddAsync(entity);
        TempData["StatusMessage"] = $"Added {entity.Name} to the FutureV catalog.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        var model = new CarInputModel
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
                    Id = image.Id,
                    ViewAngle = image.ViewAngle,
                    ImageUrl = image.ImageUrl,
                    Description = image.Description,
                    HasStoredImage = image.ImageData is { Length: > 0 },
                    ExistingImageSource = image.GetImageSource()
                })
                .ToList()
        };

        if (model.Images.Count == 0)
        {
            model.Images.Add(new CarImageInputModel { ViewAngle = "Front" });
        }

        ViewBag.Id = id;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, CarInputModel car)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Id = id;
            return View(car);
        }

        var entity = await _repository.GetByIdAsync(id);

        if (entity is null)
        {
            return NotFound();
        }

        entity.Name = car.Name;
        entity.Tagline = car.Tagline;
        entity.BasePrice = car.BasePrice;
        entity.DriveType = car.DriveType;
        entity.EnergySystem = car.EnergySystem;
        entity.AutonomyLevel = car.AutonomyLevel;
        entity.PowerOutput = car.PowerOutput;
        entity.RangePerCharge = car.RangePerCharge;
        entity.TopSpeed = car.TopSpeed;
        entity.ZeroToSixty = car.ZeroToSixty;
        entity.FastChargeMinutes = car.FastChargeMinutes;
        entity.SeatingCapacity = car.SeatingCapacity;
        entity.ReleaseYear = car.ReleaseYear;
        entity.Narrative = car.Narrative;

        var existingImages = entity.Images.ToDictionary(image => image.Id);
        var updatedImages = new List<CarImage>();

        foreach (var image in car.Images)
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
            else if (image.HasStoredImage && image.Id.HasValue && existingImages.TryGetValue(image.Id.Value, out var existingImage))
            {
                galleryImage.ImageUrl = existingImage.ImageUrl;
                galleryImage.ImageData = existingImage.ImageData;
                galleryImage.ContentType = existingImage.ContentType;
                galleryImage.FileName = existingImage.FileName;
            }

            updatedImages.Add(galleryImage);
        }

        if (updatedImages.Count == 0)
        {
            ModelState.AddModelError("Images", "Add at least one image for the vehicle gallery.");
            ViewBag.Id = id;
            return View(car);
        }

        entity.Images.Clear();
        foreach (var image in updatedImages)
        {
            entity.Images.Add(image);
        }

        await _repository.UpdateAsync(entity);
        TempData["StatusMessage"] = $"Updated {entity.Name}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var car = await _repository.GetByIdAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        return View(car);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var car = await _repository.GetByIdAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        await _repository.DeleteAsync(id);

        TempData["StatusMessage"] = $"Removed {car.Name} from the catalog.";
        return RedirectToAction(nameof(Index));
    }
}
