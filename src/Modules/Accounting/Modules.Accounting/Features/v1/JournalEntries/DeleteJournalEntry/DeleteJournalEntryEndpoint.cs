using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.JournalEntries.DeleteJournalEntry;

public static class DeleteJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapDeleteJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteJournalEntryCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteJournalEntryEndpoint))
        .WithSummary("Delete JournalEntry")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntries.Delete);
    }
}
