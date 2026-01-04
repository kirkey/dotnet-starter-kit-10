namespace FSH.Module.Microfinance.Features.v1.AmlAlerts.CreateAmlAlert;

public class CreateAmlAlertValidator : AbstractValidator<CreateAmlAlertCommand>
{
    public CreateAmlAlertValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
