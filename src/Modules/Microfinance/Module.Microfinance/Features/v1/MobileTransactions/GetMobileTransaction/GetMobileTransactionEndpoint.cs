using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MobileTransactions.GetMobileTransaction;
using FSH.Module.Microfinance.Contracts.v1.MobileTransactions;

namespace FSH.Module.Microfinance.Features.v1.MobileTransactions.GetMobileTransaction;

public static class GetMobileTransactionEndpoint
{
    public static RouteHandlerBuilder MapGetMobileTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMobileTransactionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMobileTransactionEndpoint))
        .WithSummary("Get MobileTransaction")
        .Produces<MobileTransactionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MobileTransactions.View);
    }
}
