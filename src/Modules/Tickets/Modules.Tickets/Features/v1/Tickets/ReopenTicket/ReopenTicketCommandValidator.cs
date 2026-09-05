using FluentValidation;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace FSH.Modules.Tickets.Features.v1.Tickets.ReopenTicket;

public sealed class ReopenTicketCommandValidator : AbstractValidator<ReopenTicketCommand>
{
    public ReopenTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
