using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.ApprovePostingBatch;

public class ApprovePostingBatchValidator : AbstractValidator<ApprovePostingBatchCommand>
{
    public ApprovePostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
