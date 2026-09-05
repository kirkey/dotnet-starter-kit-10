using FluentValidation;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace FSH.Modules.Tickets.Features.v1.Tickets.RestoreTicket;

public sealed class RestoreTicketCommandValidator : AbstractValidator<RestoreTicketCommand>
{
    public RestoreTicketCommandValidator()
    {
        RuleFor(x => x.TicketId).NotEmpty();
    }
}
