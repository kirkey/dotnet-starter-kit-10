using FluentValidation;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.CreateWriteOff;

public class CreateWriteOffValidator : AbstractValidator<CreateWriteOffCommand>
{
    public CreateWriteOffValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
