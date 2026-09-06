using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Chat;

namespace FSH.Modules.Ai.Features.v1.Chat.CreateChatSession;

public sealed class CreateChatSessionCommandValidator : AbstractValidator<CreateChatSessionCommand>
{
    public CreateChatSessionCommandValidator()
    {
        RuleFor(x => x.Title).MaximumLength(200).When(x => x.Title is not null);
        RuleFor(x => x.Model).MaximumLength(256).When(x => x.Model is not null);
        RuleFor(x => x.Variant).IsInEnum().When(x => x.Variant.HasValue);
    }
}
