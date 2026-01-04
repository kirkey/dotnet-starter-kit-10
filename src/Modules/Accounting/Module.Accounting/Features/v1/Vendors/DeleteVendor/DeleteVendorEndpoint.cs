using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Vendors.DeleteVendor;

public static class DeleteVendorEndpoint
{
    public static RouteHandlerBuilder MapDeleteVendorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteVendorCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteVendorEndpoint))
        .WithSummary("Delete Vendor")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Vendors.Delete);
    }
}
