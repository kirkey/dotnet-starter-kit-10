using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FuelConsumption.UpdateFuelConsumption;

public static class UpdateFuelConsumptionEndpoint
{
    public static RouteHandlerBuilder MapUpdateFuelConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            CancellationToken ct) =>
        {
            return TypedResults.StatusCode(StatusCodes.Status501NotImplemented);
        })
        .WithName(nameof(UpdateFuelConsumptionEndpoint))
        .WithSummary("Update FuelConsumption (not implemented)")
        .Produces(StatusCodes.Status501NotImplemented)
        .RequirePermission(AccountingPermissionConstants.Consumption.Update);
    }
}