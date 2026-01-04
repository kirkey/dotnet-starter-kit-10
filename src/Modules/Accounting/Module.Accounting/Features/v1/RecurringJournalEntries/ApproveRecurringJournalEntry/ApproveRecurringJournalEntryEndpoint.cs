// TODO: Implement Approve endpoint for RecurringJournalEntry
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

public static class ApproveRecurringJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapApproveRecurringJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveRecurringJournalEntryCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveRecurringJournalEntryEndpoint))
        .WithSummary("Approve RecurringJournalEntry")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.Approve);
    }
}
