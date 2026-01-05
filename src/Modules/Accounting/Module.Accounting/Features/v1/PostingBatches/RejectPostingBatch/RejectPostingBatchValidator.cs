using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.RejectPostingBatch;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.RejectPostingBatch;

public class RejectPostingBatchValidator : AbstractValidator<RejectPostingBatchCommand>
{
    public RejectPostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
