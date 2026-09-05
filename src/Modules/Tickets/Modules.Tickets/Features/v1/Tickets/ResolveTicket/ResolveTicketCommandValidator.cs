using FluentValidation;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace FSH.Modules.Tickets.Features.v1.Tickets.ResolveTicket;

public sealed class ResolveTicketCommandValidator : AbstractValidator<ResolveTicketCommand>
{
    public ResolveTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
