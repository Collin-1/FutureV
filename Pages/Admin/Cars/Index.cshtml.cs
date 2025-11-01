using FutureV.Data;
using FutureV.Models;
using FutureV.Services;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Pages.Admin.Cars;

public class IndexModel : AdminPageModel
{
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context, AdminAccessService adminAccessService) : base(adminAccessService)
    {
        _context = context;
    }

    public IList<Car> Cars { get; private set; } = [];

    public async Task OnGetAsync()
    {
        Cars = await _context.Cars
            .Include(car => car.Images)
            .OrderBy(car => car.Name)
            .ToListAsync();
    }
}
