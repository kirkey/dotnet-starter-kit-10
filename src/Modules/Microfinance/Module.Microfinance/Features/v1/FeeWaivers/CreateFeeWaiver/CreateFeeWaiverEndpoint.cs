using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeeWaivers.CreateFeeWaiver;

namespace FSH.Module.Microfinance.Features.v1.FeeWaivers.CreateFeeWaiver;

public static class CreateFeeWaiverEndpoint
{
    public static RouteHandlerBuilder MapCreateFeeWaiverEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateFeeWaiverCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateFeeWaiverEndpoint))
        .WithSummary("Create FeeWaiver")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.FeeWaivers.Create);
    }
}
