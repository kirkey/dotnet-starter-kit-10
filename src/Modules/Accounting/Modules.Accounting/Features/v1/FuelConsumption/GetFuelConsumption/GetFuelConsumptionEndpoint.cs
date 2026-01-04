using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FuelConsumption.GetFuelConsumption;

public static class GetFuelConsumptionEndpoint
{
    public static RouteHandlerBuilder MapGetFuelConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            CancellationToken ct) =>
        {
            return TypedResults.StatusCode(StatusCodes.Status501NotImplemented);
        })
        .WithName(nameof(GetFuelConsumptionEndpoint))
        .WithSummary("Get FuelConsumption by ID (not implemented)")
        .Produces(StatusCodes.Status501NotImplemented)
        .RequirePermission(AccountingPermissionConstants.Consumption.View);
    }
}