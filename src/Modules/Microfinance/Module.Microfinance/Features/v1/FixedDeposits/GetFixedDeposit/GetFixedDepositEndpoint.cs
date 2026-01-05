using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FixedDeposits.GetFixedDeposit;
using FSH.Module.Microfinance.Contracts.v1.FixedDeposits;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.GetFixedDeposit;

public static class GetFixedDepositEndpoint
{
    public static RouteHandlerBuilder MapGetFixedDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFixedDepositQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFixedDepositEndpoint))
        .WithSummary("Get FixedDeposit")
        .Produces<FixedDepositDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FixedDeposits.View);
    }
}
