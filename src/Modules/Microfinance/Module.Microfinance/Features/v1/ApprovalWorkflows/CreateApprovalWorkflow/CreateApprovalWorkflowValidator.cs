namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.CreateApprovalWorkflow;

public class CreateApprovalWorkflowValidator : AbstractValidator<CreateApprovalWorkflowCommand>
{
    public CreateApprovalWorkflowValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
