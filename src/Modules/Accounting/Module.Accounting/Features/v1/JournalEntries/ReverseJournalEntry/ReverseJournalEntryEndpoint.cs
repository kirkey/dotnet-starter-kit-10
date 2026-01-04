// TODO: Implement Reverse endpoint for JournalEntry
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

public static class ReverseJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapReverseJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReverseJournalEntryCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReverseJournalEntryEndpoint))
        .WithSummary("Reverse JournalEntry")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.Reverse);
    }
}
