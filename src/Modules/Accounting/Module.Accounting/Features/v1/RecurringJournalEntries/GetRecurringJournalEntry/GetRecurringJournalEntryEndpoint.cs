using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.GetRecurringJournalEntry;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GetRecurringJournalEntry;

public static class GetRecurringJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapGetRecurringJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRecurringJournalEntryQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRecurringJournalEntryEndpoint))
        .WithSummary("Get RecurringJournalEntry by ID")
        .Produces<RecurringJournalEntryDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.View);
    }
}
