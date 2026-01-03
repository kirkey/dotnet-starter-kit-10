using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.ShareTransactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.ShareTransactions.GetShareTransaction;

public static class GetShareTransactionEndpoint
{
    public static RouteHandlerBuilder MapGetShareTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareTransactionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareTransactionEndpoint))
        .WithSummary("Get ShareTransaction")
        .Produces<ShareTransactionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareTransactions.View);
    }
}
