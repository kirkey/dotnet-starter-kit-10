using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.ShareAccounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.GetShareAccount;

public static class GetShareAccountEndpoint
{
    public static RouteHandlerBuilder MapGetShareAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetShareAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetShareAccountEndpoint))
        .WithSummary("Get ShareAccount")
        .Produces<ShareAccountDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ShareAccounts.View);
    }
}
