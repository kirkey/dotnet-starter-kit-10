using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions.DeleteUssdSession;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.DeleteUssdSession;

public static class DeleteUssdSessionEndpoint
{
    public static RouteHandlerBuilder MapDeleteUssdSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteUssdSessionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteUssdSessionEndpoint))
        .WithSummary("Delete UssdSession")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.UssdSessions.Delete);
    }
}
