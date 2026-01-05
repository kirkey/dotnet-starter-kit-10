using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InterestRateChanges.CreateInterestRateChange;

namespace FSH.Module.Microfinance.Features.v1.InterestRateChanges.CreateInterestRateChange;

public static class CreateInterestRateChangeEndpoint
{
    public static RouteHandlerBuilder MapCreateInterestRateChangeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInterestRateChangeCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateInterestRateChangeEndpoint))
        .WithSummary("Create InterestRateChange")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.InterestRateChanges.Create);
    }
}
