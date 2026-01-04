// TODO: Implement Post endpoint for JournalEntry
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.PostJournalEntry;

public static class PostJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapPostJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new PostJournalEntryCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(PostJournalEntryEndpoint))
        .WithSummary("Post JournalEntry")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.Post);
    }
}
