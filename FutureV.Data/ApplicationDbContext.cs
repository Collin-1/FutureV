using FutureV.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<IdentityUser>(options)
{
    public DbSet<Car> Cars => Set<Car>();
    public DbSet<CarImage> CarImages => Set<CarImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Car>(entity =>
        {
            entity.Property(car => car.BasePrice).HasPrecision(18, 2);
            entity.Property(car => car.Narrative).HasMaxLength(2000);
        });

        modelBuilder.Entity<CarImage>(entity =>
        {
            entity.Property(image => image.ImageUrl).HasMaxLength(500);
            entity.Property(image => image.ContentType).HasMaxLength(120);
            entity.Property(image => image.FileName).HasMaxLength(260);
            entity.HasOne(image => image.Car)
                  .WithMany(car => car.Images)
                  .HasForeignKey(image => image.CarId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
