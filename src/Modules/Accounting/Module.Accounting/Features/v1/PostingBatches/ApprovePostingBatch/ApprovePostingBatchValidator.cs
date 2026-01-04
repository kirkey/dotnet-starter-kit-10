using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.ApprovePostingBatch;

public class ApprovePostingBatchValidator : AbstractValidator<ApprovePostingBatchCommand>
{
    public ApprovePostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
