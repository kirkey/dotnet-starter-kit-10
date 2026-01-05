using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions.UpdateUssdSession;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.UpdateUssdSession;

public static class UpdateUssdSessionEndpoint
{
    public static RouteHandlerBuilder MapUpdateUssdSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateUssdSessionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateUssdSessionEndpoint))
        .WithSummary("Update UssdSession")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.UssdSessions.Update);
    }
}
