using FSH.Module.Microfinance.Contracts.v1.GroupMemberships.CreateGroupMembership;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.CreateGroupMembership;

public class CreateGroupMembershipValidator : AbstractValidator<CreateGroupMembershipCommand>
{
    public CreateGroupMembershipValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
