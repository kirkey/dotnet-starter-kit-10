using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.CreateCustomerSurvey;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.CreateCustomerSurvey;

public class CreateCustomerSurveyValidator : AbstractValidator<CreateCustomerSurveyCommand>
{
    public CreateCustomerSurveyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
