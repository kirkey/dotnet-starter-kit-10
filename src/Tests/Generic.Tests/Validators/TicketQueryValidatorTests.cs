using FSH.Modules.Tickets.Features.v1.Tickets.SearchTickets;
using FSH.Modules.Tickets.Features.v1.Tickets.ListTrashedTickets;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace Generic.Tests.Validators;

public sealed class TicketQueryValidatorTests
{
    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 0)]
    [InlineData(1, 201)]
    public void SearchTickets_rejects_out_of_range_paging(int page, int size)
        => new SearchTicketsQueryValidator().Validate(new SearchTicketsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Fact]
    public void SearchTickets_accepts_valid_query()
        => new SearchTicketsQueryValidator().Validate(new SearchTicketsQuery()).IsValid.ShouldBeTrue();

    [Theory]
    [InlineData(0, 20)]
    [InlineData(1, 201)]
    public void ListTrashedTickets_rejects_out_of_range_paging(int page, int size)
        => new ListTrashedTicketsQueryValidator().Validate(new ListTrashedTicketsQuery() { PageNumber = page, PageSize = size }).IsValid.ShouldBeFalse();

    [Fact]
    public void ListTrashedTickets_accepts_valid_query()
        => new ListTrashedTicketsQueryValidator().Validate(new ListTrashedTicketsQuery()).IsValid.ShouldBeTrue();
}
