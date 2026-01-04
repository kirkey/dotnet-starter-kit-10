using FluentValidation;
using FSH.Module.Identity.Contracts.v1.Users.ForgotPassword;

namespace FSH.Module.Identity.Features.v1.Users.ForgotPassword;

public class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
{
    public ForgotPasswordCommandValidator()
    {
        RuleFor(p => p.Email).Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();
    }
}