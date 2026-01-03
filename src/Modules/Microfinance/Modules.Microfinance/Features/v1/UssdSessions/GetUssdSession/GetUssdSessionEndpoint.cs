using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.UssdSessions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.UssdSessions.GetUssdSession;

public static class GetUssdSessionEndpoint
{
    public static RouteHandlerBuilder MapGetUssdSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetUssdSessionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetUssdSessionEndpoint))
        .WithSummary("Get UssdSession")
        .Produces<UssdSessionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.UssdSessions.View);
    }
}
