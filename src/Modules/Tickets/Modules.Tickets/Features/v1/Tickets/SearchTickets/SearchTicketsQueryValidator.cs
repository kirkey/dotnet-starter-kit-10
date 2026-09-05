using FluentValidation;
using FSH.Modules.Tickets.Contracts.v1.Tickets;

namespace FSH.Modules.Tickets.Features.v1.Tickets.SearchTickets;

public sealed class SearchTicketsQueryValidator : AbstractValidator<SearchTicketsQuery>
{
    public SearchTicketsQueryValidator()
    {
        // Bounds mirror the handler's clamp window (page < 1 → 1, size outside 1..200 → 20).
        RuleFor(q => q.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(q => q.PageSize)
            .InclusiveBetween(1, 200)
            .WithMessage("Page size must be between 1 and 200.");
    }
}
