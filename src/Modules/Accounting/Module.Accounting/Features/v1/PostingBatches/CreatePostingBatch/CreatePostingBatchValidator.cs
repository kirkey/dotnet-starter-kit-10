using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.CreatePostingBatch;

public class CreatePostingBatchValidator : AbstractValidator<CreatePostingBatchCommand>
{
    public CreatePostingBatchValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => x.BatchDate.HasValue, () =>
        {
            RuleFor(x => x.BatchDate!.Value)
                .LessThanOrEqualTo(DateTime.UtcNow.Date).WithMessage("BatchDate cannot be in the future");
        });
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
