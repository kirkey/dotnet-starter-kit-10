// TODO: Implement Complete endpoint for FiscalPeriodClose
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;

public record CompleteFiscalPeriodCloseRequest(Guid ClosingJournalEntryId);

public static class CompleteFiscalPeriodCloseEndpoint
{
    public static RouteHandlerBuilder MapCompleteFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            CompleteFiscalPeriodCloseRequest request,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new CompleteFiscalPeriodCloseCommand(id, request.ClosingJournalEntryId), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(CompleteFiscalPeriodCloseEndpoint))
        .WithSummary("Complete FiscalPeriodClose")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FiscalPeriodClose.Complete);
    }
}
