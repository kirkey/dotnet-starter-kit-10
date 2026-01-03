namespace FSH.Modules.Microfinance.Features.v1.BranchTargets.CreateBranchTarget;

public class CreateBranchTargetValidator : AbstractValidator<CreateBranchTargetCommand>
{
    public CreateBranchTargetValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
