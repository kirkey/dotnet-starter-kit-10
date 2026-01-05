using FluentValidation;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.PostPostingBatch;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.PostPostingBatch;

public class PostPostingBatchValidator : AbstractValidator<PostPostingBatchCommand>
{
    public PostPostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
