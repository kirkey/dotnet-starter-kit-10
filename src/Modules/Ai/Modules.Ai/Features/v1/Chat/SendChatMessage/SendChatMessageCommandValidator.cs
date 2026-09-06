using FluentValidation;
using FSH.Modules.Ai.Contracts.v1.Chat;

namespace FSH.Modules.Ai.Features.v1.Chat.SendChatMessage;

public sealed class SendChatMessageCommandValidator : AbstractValidator<SendChatMessageCommand>
{
    public SendChatMessageCommandValidator()
    {
        RuleFor(x => x.SessionId).NotEmpty();
        RuleFor(x => x.Content).NotEmpty().MaximumLength(8000);
    }
}
