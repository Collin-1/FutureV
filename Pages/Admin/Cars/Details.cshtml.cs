using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Admin.Cars;

public class DetailsModel : AdminPageModel
{
    private readonly ApplicationDbContext _context;

    public DetailsModel(ApplicationDbContext context, AdminAccessService adminAccessService) : base(adminAccessService)
    {
        _context = context;
    }

    public Car? Car { get; private set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        Car = await _context.Cars
            .Include(car => car.Images)
            .FirstOrDefaultAsync(car => car.Id == id);

        if (Car is null)
        {
            return NotFound();
        }

        return Page();
    }
}
