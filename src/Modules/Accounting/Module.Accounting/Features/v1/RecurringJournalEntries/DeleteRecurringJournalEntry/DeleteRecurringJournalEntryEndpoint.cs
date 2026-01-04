using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;

public static class DeleteRecurringJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapDeleteRecurringJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRecurringJournalEntryCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteRecurringJournalEntryEndpoint))
        .WithSummary("Delete RecurringJournalEntry")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.Delete);
    }
}
