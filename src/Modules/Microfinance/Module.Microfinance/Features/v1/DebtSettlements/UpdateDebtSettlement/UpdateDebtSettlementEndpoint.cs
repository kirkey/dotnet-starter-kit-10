using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.UpdateDebtSettlement;

public static class UpdateDebtSettlementEndpoint
{
    public static RouteHandlerBuilder MapUpdateDebtSettlementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateDebtSettlementCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateDebtSettlementEndpoint))
        .WithSummary("Update DebtSettlement")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.DebtSettlements.Update);
    }
}
