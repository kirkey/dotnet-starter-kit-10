using FSH.Modules.Tickets.Features.v1.Tickets.RestoreTicket;
using FSH.Modules.Tickets.Features.v1.Tickets.ResolveTicket;
using FSH.Modules.Tickets.Features.v1.Tickets.ReopenTicket;
using FSH.Modules.Tickets.Features.v1.Tickets.AssignTicket;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace Generic.Tests.Validators;

public sealed class TicketCommandValidatorTests
{
    [Fact]
    public void RestoreTicket_rejects_empty_id()
        => new RestoreTicketCommandValidator().Validate(new RestoreTicketCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void ResolveTicket_rejects_empty_id()
        => new ResolveTicketCommandValidator().Validate(new ResolveTicketCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void ReopenTicket_rejects_empty_id()
        => new ReopenTicketCommandValidator().Validate(new ReopenTicketCommand(Guid.Empty)).IsValid.ShouldBeFalse();

    [Fact]
    public void AssignTicket_rejects_empty_id()
        => new AssignTicketCommandValidator().Validate(new AssignTicketCommand(Guid.Empty, null)).IsValid.ShouldBeFalse();

    [Fact]
    public void Accepts_valid_ids()
    {
        new RestoreTicketCommandValidator().Validate(new RestoreTicketCommand(Guid.NewGuid())).IsValid.ShouldBeTrue();
        new AssignTicketCommandValidator().Validate(new AssignTicketCommand(Guid.NewGuid(), Guid.NewGuid())).IsValid.ShouldBeTrue();
    }
}
