using FSH.Module.Microfinance.Contracts.v1.BranchTargets.CreateBranchTarget;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.CreateBranchTarget;

public class CreateBranchTargetValidator : AbstractValidator<CreateBranchTargetCommand>
{
    public CreateBranchTargetValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
