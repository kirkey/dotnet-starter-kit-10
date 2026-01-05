using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.JournalEntryLines;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.GetJournalEntryLine;

public static class GetJournalEntryLineEndpoint
{
    public static RouteHandlerBuilder MapGetJournalEntryLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new FSH.Module.Accounting.Contracts.v1.JournalEntryLines.GetJournalEntryLine.GetJournalEntryLineQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetJournalEntryLineEndpoint))
        .WithSummary("Get JournalEntryLine by ID")
        .Produces<JournalEntryLineDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntryLines.View);
    }
}
