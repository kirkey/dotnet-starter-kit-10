using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.RecurringJournalEntries;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.RecurringJournalEntries.GetRecurringJournalEntries;

public static class GetRecurringJournalEntriesEndpoint
{
    public static RouteHandlerBuilder MapGetRecurringJournalEntriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetRecurringJournalEntriesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRecurringJournalEntriesEndpoint))
        .WithSummary("Get paginated list of RecurringJournalEntries")
        .Produces<RecurringJournalEntriesPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.Search);
    }
}
