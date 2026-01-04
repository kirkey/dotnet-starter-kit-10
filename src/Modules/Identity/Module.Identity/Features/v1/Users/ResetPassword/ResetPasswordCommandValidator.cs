using FluentValidation;
using FSH.Module.Identity.Contracts.v1.Users.ResetPassword;

namespace FSH.Module.Identity.Features.v1.Users.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}