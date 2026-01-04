using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.RejectPostingBatch;

public class RejectPostingBatchValidator : AbstractValidator<RejectPostingBatchCommand>
{
    public RejectPostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
