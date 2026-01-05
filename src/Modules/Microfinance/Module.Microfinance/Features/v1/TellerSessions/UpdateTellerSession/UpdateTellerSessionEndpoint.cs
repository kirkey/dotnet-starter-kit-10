using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.UpdateTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.UpdateTellerSession;

public static class UpdateTellerSessionEndpoint
{
    public static RouteHandlerBuilder MapUpdateTellerSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateTellerSessionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateTellerSessionEndpoint))
        .WithSummary("Update TellerSession")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.TellerSessions.Update);
    }
}
