namespace FSH.Modules.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;

public class CreateGroupMembershipValidator : AbstractValidator<CreateGroupMembershipCommand>
{
    public CreateGroupMembershipValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
