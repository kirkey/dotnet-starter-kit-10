using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.DebtSettlements.DeleteDebtSettlement;

namespace FSH.Module.Microfinance.Features.v1.DebtSettlements.DeleteDebtSettlement;

public static class DeleteDebtSettlementEndpoint
{
    public static RouteHandlerBuilder MapDeleteDebtSettlementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteDebtSettlementCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteDebtSettlementEndpoint))
        .WithSummary("Delete DebtSettlement")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.DebtSettlements.Delete);
    }
}
