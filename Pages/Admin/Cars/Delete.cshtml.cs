using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Admin.Cars;

public class DeleteModel : AdminPageModel
{
    private readonly ApplicationDbContext _context;

    public DeleteModel(ApplicationDbContext context, AdminAccessService adminAccessService) : base(adminAccessService)
    {
        _context = context;
    }

    public Car? Car { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Car = await _context.Cars
            .AsNoTracking()
            .FirstOrDefaultAsync(car => car.Id == id);

        if (Car is null)
        {
            return NotFound();
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);

        if (car is null)
        {
            return NotFound();
        }

        _context.Cars.Remove(car);
        await _context.SaveChangesAsync();

        TempData["StatusMessage"] = $"Removed {car.Name} from the catalog.";
        return RedirectToPage("Index");
    }
}
