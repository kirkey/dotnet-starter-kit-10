using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Vendors.CreateVendor;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.Vendors.CreateVendor;

namespace FSH.Module.Accounting.Features.v1.Vendors.CreateVendor;

public static class CreateVendorEndpoint
{
    public static RouteHandlerBuilder MapCreateVendorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateVendorCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/vendors/{id}", id);
        })
        .WithName(nameof(CreateVendorEndpoint))
        .WithSummary("Create Vendor")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Vendors.Create);
    }
}
