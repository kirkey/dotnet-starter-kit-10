using FluentValidation;
using FSH.Module.Identity.Contracts.v1.Tokens.TokenGeneration;

namespace FSH.Module.Identity.Features.v1.Tokens.TokenGeneration;

public class TokenGenerationCommandValidator : AbstractValidator<GenerateTokenCommand>
{
    public TokenGenerationCommandValidator()
    {
        RuleFor(p => p.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .EmailAddress();

        RuleFor(p => p.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty();
    }
}