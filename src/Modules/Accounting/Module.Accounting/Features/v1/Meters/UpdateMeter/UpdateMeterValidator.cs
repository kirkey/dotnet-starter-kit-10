using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Meters.UpdateMeter;

namespace FSH.Module.Accounting.Features.v1.Meters.UpdateMeter;

public class UpdateMeterValidator : AbstractValidator<UpdateMeterCommand>
{
    public UpdateMeterValidator()
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
