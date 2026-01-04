namespace FSH.Module.Microfinance.Features.v1.MemberGroups.CreateMemberGroup;

public class CreateMemberGroupValidator : AbstractValidator<CreateMemberGroupCommand>
{
    public CreateMemberGroupValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
