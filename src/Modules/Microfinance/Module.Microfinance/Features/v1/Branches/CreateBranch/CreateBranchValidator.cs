namespace FSH.Module.Microfinance.Features.v1.Branches.CreateBranch;

public class CreateBranchValidator : AbstractValidator<CreateBranchCommand>
{
    public CreateBranchValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
