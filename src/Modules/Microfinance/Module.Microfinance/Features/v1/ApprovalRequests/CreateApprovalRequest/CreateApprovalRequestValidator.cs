using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.CreateApprovalRequest;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.CreateApprovalRequest;

public class CreateApprovalRequestValidator : AbstractValidator<CreateApprovalRequestCommand>
{
    public CreateApprovalRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
