using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.JournalEntryLines.DeleteJournalEntryLine;

public static class DeleteJournalEntryLineEndpoint
{
    public static RouteHandlerBuilder MapDeleteJournalEntryLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteJournalEntryLineCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteJournalEntryLineEndpoint))
        .WithSummary("Delete JournalEntryLine")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntryLines.Delete);
    }
}
