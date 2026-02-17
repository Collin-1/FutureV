namespace FutureV.ViewModels;

public record CarSummaryViewModel(
    int Id,
    string Name,
    string Tagline,
    decimal BasePrice,
    string Autonomy,
    int Range,
    int TopSpeed,
    double ZeroToSixty,
    string? HeroImageSrc
);