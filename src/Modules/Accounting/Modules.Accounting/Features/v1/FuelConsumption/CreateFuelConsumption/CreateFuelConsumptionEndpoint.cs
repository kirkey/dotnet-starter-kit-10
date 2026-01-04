using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FuelConsumption.CreateFuelConsumption;

public static class CreateFuelConsumptionEndpoint
{
    public static RouteHandlerBuilder MapCreateFuelConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CancellationToken ct) =>
        {
            return TypedResults.StatusCode(StatusCodes.Status501NotImplemented);
        })
        .WithName(nameof(CreateFuelConsumptionEndpoint))
        .WithSummary("Create FuelConsumption (not implemented)")
        .Produces(StatusCodes.Status501NotImplemented)
        .RequirePermission(AccountingPermissionConstants.Consumption.Create);
    }
}