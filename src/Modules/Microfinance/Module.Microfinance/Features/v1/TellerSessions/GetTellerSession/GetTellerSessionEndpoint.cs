using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.GetTellerSession;
using FSH.Module.Microfinance.Contracts.v1.TellerSessions;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.GetTellerSession;

public static class GetTellerSessionEndpoint
{
    public static RouteHandlerBuilder MapGetTellerSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTellerSessionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTellerSessionEndpoint))
        .WithSummary("Get TellerSession")
        .Produces<TellerSessionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.TellerSessions.View);
    }
}
