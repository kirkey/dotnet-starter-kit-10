using FluentValidation;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace FSH.Modules.Tickets.Features.v1.Tickets.AssignTicket;

public sealed class AssignTicketCommandValidator : AbstractValidator<AssignTicketCommand>
{
    public AssignTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
