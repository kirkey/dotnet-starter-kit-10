namespace FSH.Modules.Microfinance.Features.v1.ReportGenerations.CreateReportGeneration;

public class CreateReportGenerationValidator : AbstractValidator<CreateReportGenerationCommand>
{
    public CreateReportGenerationValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
