// TODO: Implement Reopen endpoint for AccountingPeriod
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.AccountingPeriods.ReopenAccountingPeriod;

public static class ReopenAccountingPeriodEndpoint
{
    public static RouteHandlerBuilder MapReopenAccountingPeriodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReopenAccountingPeriodCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReopenAccountingPeriodEndpoint))
        .WithSummary("Reopen AccountingPeriod")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountingPeriods.Reopen);
    }
}
