using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.DeleteTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.DeleteTellerSession;

public static class DeleteTellerSessionEndpoint
{
    public static RouteHandlerBuilder MapDeleteTellerSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteTellerSessionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteTellerSessionEndpoint))
        .WithSummary("Delete TellerSession")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.TellerSessions.Delete);
    }
}
