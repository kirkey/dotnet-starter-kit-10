using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Consumption.UpdateConsumption;

namespace FSH.Module.Accounting.Features.v1.Consumption.UpdateConsumption;

public class UpdateConsumptionValidator : AbstractValidator<UpdateConsumptionCommand>
{
    public UpdateConsumptionValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
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
