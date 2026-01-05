using FluentValidation;
using FSH.Module.Accounting.Features;
using FSH.Module.Accounting.Contracts.v1.Members.CreateMember;

namespace FSH.Module.Accounting.Features.v1.Members.CreateMember;

public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.Name)
            .ValidateMemberName();
            
        RuleFor(x => x.Description)
            .ValidateDescription();
    }
}
