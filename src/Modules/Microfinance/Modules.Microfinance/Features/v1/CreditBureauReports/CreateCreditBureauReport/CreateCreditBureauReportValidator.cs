namespace FSH.Modules.Microfinance.Features.v1.CreditBureauReports.CreateCreditBureauReport;

public class CreateCreditBureauReportValidator : AbstractValidator<CreateCreditBureauReportCommand>
{
    public CreateCreditBureauReportValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
