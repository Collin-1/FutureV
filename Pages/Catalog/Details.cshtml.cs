using FutureV.Data;
using FutureV.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Catalog;

public class DetailsModel(ApplicationDbContext context) : PageModel
{
    private readonly ApplicationDbContext _context = context;

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
