using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.TellerSessions.CreateTellerSession;

namespace FSH.Module.Microfinance.Features.v1.TellerSessions.CreateTellerSession;

public static class CreateTellerSessionEndpoint
{
    public static RouteHandlerBuilder MapCreateTellerSessionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateTellerSessionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateTellerSessionEndpoint))
        .WithSummary("Create TellerSession")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.TellerSessions.Create);
    }
}
