using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FuelConsumption.DeleteFuelConsumption;

public static class DeleteFuelConsumptionEndpoint
{
    public static RouteHandlerBuilder MapDeleteFuelConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            CancellationToken ct) =>
        {
            return TypedResults.StatusCode(StatusCodes.Status501NotImplemented);
        })
        .WithName(nameof(DeleteFuelConsumptionEndpoint))
        .WithSummary("Delete FuelConsumption (not implemented)")
        .Produces(StatusCodes.Status501NotImplemented)
        .RequirePermission(AccountingPermissionConstants.Consumption.Delete);
    }
}