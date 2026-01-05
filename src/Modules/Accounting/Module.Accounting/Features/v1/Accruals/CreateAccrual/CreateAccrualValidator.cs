using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.Accruals.CreateAccrual;
using FSH.Module.Accounting.Features;

namespace FSH.Module.Accounting.Features.v1.Accruals.CreateAccrual;

public class CreateAccrualValidator : AbstractValidator<CreateAccrualCommand>
{
    public CreateAccrualValidator()
    {
        RuleFor(x => x.Name)
            .ValidateName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
