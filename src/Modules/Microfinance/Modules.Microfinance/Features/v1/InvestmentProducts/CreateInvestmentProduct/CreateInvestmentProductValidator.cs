namespace FSH.Modules.Microfinance.Features.v1.InvestmentProducts.CreateInvestmentProduct;

public class CreateInvestmentProductValidator : AbstractValidator<CreateInvestmentProductCommand>
{
    public CreateInvestmentProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
