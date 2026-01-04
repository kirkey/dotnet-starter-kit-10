using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Vendors;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Vendors.GetVendor;

public static class GetVendorEndpoint
{
    public static RouteHandlerBuilder MapGetVendorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetVendorQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetVendorEndpoint))
        .WithSummary("Get Vendor by ID")
        .Produces<VendorDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Vendors.View);
    }
}
