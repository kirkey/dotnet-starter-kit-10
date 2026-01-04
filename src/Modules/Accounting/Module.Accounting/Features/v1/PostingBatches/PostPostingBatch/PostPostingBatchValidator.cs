using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.PostPostingBatch;

public class PostPostingBatchValidator : AbstractValidator<PostPostingBatchCommand>
{
    public PostPostingBatchValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
