namespace FSH.Modules.Microfinance.Features.v1.Staffs.CreateStaff;

public class CreateStaffValidator : AbstractValidator<CreateStaffCommand>
{
    public CreateStaffValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
