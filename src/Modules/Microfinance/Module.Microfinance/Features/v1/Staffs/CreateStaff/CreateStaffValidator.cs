using FSH.Module.Microfinance.Contracts.v1.Staffs.CreateStaff;

namespace FSH.Module.Microfinance.Features.v1.Staffs.CreateStaff;

public class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
