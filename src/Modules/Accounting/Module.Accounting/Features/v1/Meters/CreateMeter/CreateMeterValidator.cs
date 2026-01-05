using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Meters.CreateMeter;

namespace FSH.Module.Accounting.Features.v1.Meters.CreateMeter;

public class CreateMeterValidator : AbstractValidator<CreateMeterCommand>
{
    public CreateMeterValidator()
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
