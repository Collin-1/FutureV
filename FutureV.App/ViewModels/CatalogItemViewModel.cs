namespace FutureV.ViewModels;

public record CatalogItemViewModel(
    int Id,
    string Name,
    string Tagline,
    decimal BasePrice,
    string Autonomy,
    string DriveType,
    string EnergySystem,
    int Range,
    int TopSpeed,
    double ZeroToSixty,
    string? HeroImageSrc
);