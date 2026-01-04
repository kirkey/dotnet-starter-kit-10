using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.PromiseToPays;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.GetPromiseToPay;

public static class GetPromiseToPayEndpoint
{
    public static RouteHandlerBuilder MapGetPromiseToPayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPromiseToPayQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPromiseToPayEndpoint))
        .WithSummary("Get PromiseToPay")
        .Produces<PromiseToPayDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.PromiseToPays.View);
    }
}
