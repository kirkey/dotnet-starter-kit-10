namespace FSH.Modules.Microfinance.Features.v1.ReportDefinitions.CreateReportDefinition;

public class CreateReportDefinitionValidator : AbstractValidator<CreateReportDefinitionCommand>
{
    public CreateReportDefinitionValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
