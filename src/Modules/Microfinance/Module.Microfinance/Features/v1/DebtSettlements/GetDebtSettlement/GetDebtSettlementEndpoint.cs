using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.DebtSettlements.GetDebtSettlement;
using FSH.Module.Microfinance.Contracts.v1.DebtSettlements;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.GetDebtSettlement;

public static class GetDebtSettlementEndpoint
{
    public static RouteHandlerBuilder MapGetDebtSettlementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDebtSettlementQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDebtSettlementEndpoint))
        .WithSummary("Get DebtSettlement")
        .Produces<DebtSettlementDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.DebtSettlements.View);
    }
}
