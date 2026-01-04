using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.JournalEntries;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.GetJournalEntry;

public static class GetJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapGetJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetJournalEntryQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetJournalEntryEndpoint))
        .WithSummary("Get JournalEntry by ID")
        .Produces<JournalEntryDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.View);
    }
}
