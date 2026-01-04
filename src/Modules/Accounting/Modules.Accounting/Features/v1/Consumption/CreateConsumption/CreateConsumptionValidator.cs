using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.Consumption.CreateConsumption;

public class CreateConsumptionValidator : AbstractValidator<CreateConsumptionCommand>
{
    public CreateConsumptionValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
