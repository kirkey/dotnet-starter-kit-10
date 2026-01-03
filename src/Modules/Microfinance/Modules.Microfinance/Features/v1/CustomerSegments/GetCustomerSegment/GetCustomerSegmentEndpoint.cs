using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CustomerSegments;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.GetCustomerSegment;

public static class GetCustomerSegmentEndpoint
{
    public static RouteHandlerBuilder MapGetCustomerSegmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerSegmentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCustomerSegmentEndpoint))
        .WithSummary("Get CustomerSegment")
        .Produces<CustomerSegmentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSegments.View);
    }
}
