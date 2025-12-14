using FutureV.Core.Entities;
using FutureV.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Data.Repositories;

public class CarRepository : ICarRepository
{
    private readonly ApplicationDbContext _context;

    public CarRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Car>> GetAllAsync()
    {
        return await _context.Cars
            .Include(c => c.Images)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Car?> GetByIdAsync(int id)
    {
        return await _context.Cars
            .Include(c => c.Images)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task AddAsync(Car car)
    {
        await _context.Cars.AddAsync(car);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Car car)
    {
        _context.Cars.Update(car);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var car = await _context.Cars.FindAsync(id);
        if (car != null)
        {
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Cars.AnyAsync(e => e.Id == id);
    }
}
