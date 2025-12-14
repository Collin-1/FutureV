using FutureV.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FutureV.Data;

public static class SeedData
{
    public static async Task InitializeAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync())
        {
            var adminUser = new IdentityUser
            {
                UserName = "admin@futurev.com",
                Email = "admin@futurev.com",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(adminUser, "FutureV@2025!");
        }

        if (await context.Cars.AnyAsync())
        {
            return;
        }

        var novaEdge = new Car
        {
            Name = "Nova Edge",
            Tagline = "Sharp lines, luminous intelligence",
            DriveType = "Quad-Motor AWD",
            EnergySystem = "Solid-state fusion hybrid",
            AutonomyLevel = "Level 5 Guardian AI",
            PowerOutput = "1,400 kW peak",
            BasePrice = 420_000m,
            RangePerCharge = 980,
            TopSpeed = 260,
            ZeroToSixty = 1.7,
            FastChargeMinutes = 12,
            SeatingCapacity = 4,
            ReleaseYear = 2032,
            Narrative = "The Nova Edge balances hyperspeed performance with adaptive comfort. It learns your driving signatures and shapes the cabin light-field to your mood."
        };

        novaEdge.Images =
        [
            new CarImage { ViewAngle = "Front", ImageUrl = "https://images.unsplash.com/photo-1493238792000-8113da705763", Description = "Nova Edge front profile under neon grid." },
            new CarImage { ViewAngle = "Side", ImageUrl = "https://images.unsplash.com/photo-1494976388531-d1058494cdd8", Description = "Low-slung silhouette highlighting aerodynamic panels." },
            new CarImage { ViewAngle = "Rear", ImageUrl = "https://images.unsplash.com/photo-1503736334956-4c8f8e92946d", Description = "Helix taillights with adaptive diffusers deployed." }
        ];

        var auroraSphere = new Car
        {
            Name = "Aurora Sphere",
            Tagline = "Glide through gravity wells with grace",
            DriveType = "Dual-axis mag-lev pods",
            EnergySystem = "Helium-3 plasma stack",
            AutonomyLevel = "Level 4 Sentient Co-Pilot",
            PowerOutput = "900 kW continuous",
            BasePrice = 360_000m,
            RangePerCharge = 860,
            TopSpeed = 240,
            ZeroToSixty = 2.3,
            FastChargeMinutes = 9,
            SeatingCapacity = 5,
            ReleaseYear = 2031,
            Narrative = "Aurora Sphere wraps passengers in a holosphere cabin and uses quantum navigation to map safe trajectories ahead of real time."
        };

        auroraSphere.Images =
        [
            new CarImage { ViewAngle = "Front Quarter", ImageUrl = "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf", Description = "Aurora Sphere gliding through aurora-lit highway." },
            new CarImage { ViewAngle = "Interior", ImageUrl = "https://images.unsplash.com/photo-1518551054981-3f35c5f6a4eb", Description = "Ambient holosphere cockpit with adaptive seating." },
            new CarImage { ViewAngle = "Charge", ImageUrl = "https://images.unsplash.com/photo-1525609004556-c46c7d6cf023", Description = "Mag-lev pods docked for plasma recharge." }
        ];

        var chronoFlux = new Car
        {
            Name = "Chrono Flux",
            Tagline = "Time-shift your commute",
            DriveType = "Tri-vector hover array",
            EnergySystem = "Temporal-loop kinetic recycler",
            AutonomyLevel = "Level 5 Predictive Navigator",
            PowerOutput = "1,050 kW pulse",
            BasePrice = 510_000m,
            RangePerCharge = 1_050,
            TopSpeed = 310,
            ZeroToSixty = 1.5,
            FastChargeMinutes = 7,
            SeatingCapacity = 2,
            ReleaseYear = 2035,
            Narrative = "Chrono Flux compresses travel time with micro-warp corridors. Its cabin features time-synced wellness cues to eliminate jet lag on terrestrial routes."
        };

        chronoFlux.Images =
        [
            new CarImage { ViewAngle = "Hover", ImageUrl = "https://images.unsplash.com/photo-1497436072909-60f360e1d4b1", Description = "Chrono Flux hovering above a mirrored runway." },
            new CarImage { ViewAngle = "Cabin", ImageUrl = "https://images.unsplash.com/photo-1503736334956-4c8f8e92946d", Description = "Dual seat cockpit with chronometric HUD." },
            new CarImage { ViewAngle = "Transit", ImageUrl = "https://images.unsplash.com/photo-1519641471654-76ce0107ad1b", Description = "Streaking through a temporal lane at dusk." }
        ];

        await context.Cars.AddRangeAsync(novaEdge, auroraSphere, chronoFlux);
        await context.SaveChangesAsync();
    }
}
