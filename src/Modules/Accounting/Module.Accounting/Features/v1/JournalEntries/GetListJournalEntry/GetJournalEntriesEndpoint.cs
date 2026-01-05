using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.JournalEntries;
using FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.GetListJournalEntry;

public static class GetJournalEntriesEndpoint
{
    public static RouteHandlerBuilder MapGetJournalEntriesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isPosted,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new FSH.Module.Accounting.Contracts.v1.JournalEntries.GetListJournalEntry.GetListJournalEntryQuery(page, pageSize, searchTerm, isPosted), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetJournalEntriesEndpoint))
        .WithSummary("Get paginated list of JournalEntries")
        .Produces<JournalEntriesPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.Search);
    }
}
