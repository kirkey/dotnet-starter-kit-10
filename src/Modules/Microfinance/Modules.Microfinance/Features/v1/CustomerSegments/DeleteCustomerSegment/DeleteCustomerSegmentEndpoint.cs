using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSegments.DeleteCustomerSegment;

public static class DeleteCustomerSegmentEndpoint
{
    public static RouteHandlerBuilder MapDeleteCustomerSegmentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCustomerSegmentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCustomerSegmentEndpoint))
        .WithSummary("Delete CustomerSegment")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSegments.Delete);
    }
}
