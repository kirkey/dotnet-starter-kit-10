using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.FixedAssets.DepreciateFixedAsset;

public class DepreciateFixedAssetValidator : AbstractValidator<DepreciateFixedAssetCommand>
{
    public DepreciateFixedAssetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        When(x => x.Amount.HasValue, () =>
        {
            RuleFor(x => x.Amount!.Value).GreaterThan(0);
        });
    }
}
