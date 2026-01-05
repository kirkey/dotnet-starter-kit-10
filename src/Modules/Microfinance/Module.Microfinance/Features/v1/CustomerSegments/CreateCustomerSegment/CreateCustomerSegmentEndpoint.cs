using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerSegments.CreateCustomerSegment;

namespace FSH.Module.Microfinance.Features.v1.CustomerSegments.CreateCustomerSegment;

public static class CreateCustomerSegmentEndpoint
{
    public static RouteHandlerBuilder MapCreateCustomerSegmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCustomerSegmentCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCustomerSegmentEndpoint))
        .WithSummary("Create CustomerSegment")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSegments.Create);
    }
}
