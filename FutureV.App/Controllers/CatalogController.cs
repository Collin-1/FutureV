using FutureV.Core.Interfaces;
using FutureV.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FutureV.Controllers;

public class CatalogController : Controller
{
    private readonly ICarRepository _repository;

    public CatalogController(ICarRepository repository)
    {
        _repository = repository;
    }

    [ResponseCache(Duration = 60, Location = ResponseCacheLocation.Any)]
    public async Task<IActionResult> Index()
    {
        var cars = await _repository.GetAllAsync();

        var model = cars.Select(car =>
        {
            var heroImage = car.Images.OrderBy(i => i.Id).FirstOrDefault();
            return new CatalogItemViewModel(
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
                heroImage?.GetImageSource()
            );
        }).ToList();

        return View(model);
    }

    [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, VaryByQueryKeys = new[] { "id" })]
    public async Task<IActionResult> Details(int id)
    {
        var car = await _repository.GetByIdAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        return View(car);
    }

}
