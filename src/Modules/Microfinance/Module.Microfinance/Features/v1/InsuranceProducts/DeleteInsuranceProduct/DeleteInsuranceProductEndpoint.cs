using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InsuranceProducts.DeleteInsuranceProduct;

public static class DeleteInsuranceProductEndpoint
{
    public static RouteHandlerBuilder MapDeleteInsuranceProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInsuranceProductCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInsuranceProductEndpoint))
        .WithSummary("Delete InsuranceProduct")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceProducts.Delete);
    }
}
