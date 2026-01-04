using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.FixedAssets.CreateFixedAsset;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.CreateFixedAsset;

public class CreateFixedAssetValidator : AbstractValidator<CreateFixedAssetCommand>
{
    public CreateFixedAssetValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });

        When(x => x.AcquisitionDate.HasValue, () =>
        {
            RuleFor(x => x.AcquisitionDate!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("Acquisition date cannot be in the future");
        });

        RuleFor(x => x.Cost)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ResidualValue)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.DepreciationRate)
            .GreaterThanOrEqualTo(0);

    }
}
