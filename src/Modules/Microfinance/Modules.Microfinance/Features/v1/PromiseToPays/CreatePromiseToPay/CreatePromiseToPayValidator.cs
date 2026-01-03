namespace FSH.Modules.Microfinance.Features.v1.PromiseToPays.CreatePromiseToPay;

public class CreatePromiseToPayValidator : AbstractValidator<CreatePromiseToPayCommand>
{
    public CreatePromiseToPayValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
