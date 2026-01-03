namespace FSH.Modules.Microfinance.Features.v1.SavingsProducts.CreateSavingsProduct;

public class CreateSavingsProductValidator : AbstractValidator<CreateSavingsProductCommand>
{
    public CreateSavingsProductValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
