using FSH.Module.Microfinance.Contracts.v1.ShareProducts.CreateShareProduct;

namespace FSH.Module.Microfinance.Features.v1.ShareProducts.CreateShareProduct;

public class CreateShareProductValidator : AbstractValidator<CreateShareProductCommand>
{
    public CreateShareProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
