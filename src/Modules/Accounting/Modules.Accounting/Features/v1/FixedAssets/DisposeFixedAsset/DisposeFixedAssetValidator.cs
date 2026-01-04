using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.FixedAssets.DisposeFixedAsset;

public class DisposeFixedAssetValidator : AbstractValidator<DisposeFixedAssetCommand>
{
    public DisposeFixedAssetValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        When(x => x.Proceeds.HasValue, () =>
        {
            RuleFor(x => x.Proceeds!.Value).GreaterThanOrEqualTo(0);
        });
    }
}
