namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.CreateMfiConfiguration;

public class CreateMfiConfigurationValidator : AbstractValidator<CreateMfiConfigurationCommand>
{
    public CreateMfiConfigurationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
