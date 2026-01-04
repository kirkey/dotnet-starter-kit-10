// TODO: Implement Generate endpoint for RecurringJournalEntry
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;

public static class GenerateRecurringJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapGenerateRecurringJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new GenerateRecurringJournalEntryCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(GenerateRecurringJournalEntryEndpoint))
        .WithSummary("Generate RecurringJournalEntry")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.Generate);
    }
}
