using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Chat;

namespace FSH.Modules.Ai.Features.v1.Chat.DeleteChatSession;

public sealed class DeleteChatSessionCommandValidator : AbstractValidator<DeleteChatSessionCommand>
{
    public DeleteChatSessionCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
    }
}
