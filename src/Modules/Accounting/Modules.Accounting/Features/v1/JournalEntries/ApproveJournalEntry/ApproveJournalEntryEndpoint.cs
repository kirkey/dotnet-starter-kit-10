// TODO: Implement Approve endpoint for JournalEntry
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;

public static class ApproveJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapApproveJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveJournalEntryCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveJournalEntryEndpoint))
        .WithSummary("Approve JournalEntry")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.Approve);
    }
}
