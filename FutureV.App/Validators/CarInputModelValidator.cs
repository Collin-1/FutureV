using FluentValidation;
using FutureV.ViewModels;

namespace FutureV.App.Validators;

public class CarInputModelValidator : AbstractValidator<CarInputModel>
{
    public CarInputModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(80);
        RuleFor(x => x.Tagline).NotEmpty().MaximumLength(120);
        RuleFor(x => x.DriveType).NotEmpty().MaximumLength(80);
        RuleFor(x => x.EnergySystem).NotEmpty().MaximumLength(80);
        RuleFor(x => x.AutonomyLevel).NotEmpty().MaximumLength(80);
        RuleFor(x => x.PowerOutput).NotEmpty().MaximumLength(80);
        RuleFor(x => x.BasePrice).InclusiveBetween(0, 10_000_000);
        RuleFor(x => x.RangePerCharge).InclusiveBetween(0, 1000);
        RuleFor(x => x.TopSpeed).InclusiveBetween(0, 500);
        RuleFor(x => x.ZeroToSixty).InclusiveBetween(0, 10);
        RuleFor(x => x.FastChargeMinutes).InclusiveBetween(0, 24);
        RuleFor(x => x.SeatingCapacity).InclusiveBetween(1, 12);
        RuleFor(x => x.ReleaseYear).InclusiveBetween(2024, 2100);
        RuleFor(x => x.Narrative).MaximumLength(2000);
    }
}
