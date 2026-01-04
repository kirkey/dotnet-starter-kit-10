using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FuelConsumption.GetListFuelConsumption;

public static class GetFuelConsumptionEndpoint
{
    public static RouteHandlerBuilder MapGetFuelConsumptionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            CancellationToken ct) =>
        {
            return TypedResults.StatusCode(StatusCodes.Status501NotImplemented);
        })
        .WithName(nameof(GetFuelConsumptionEndpoint))
        .WithSummary("Get paginated list of FuelConsumption (not implemented)")
        .Produces(StatusCodes.Status501NotImplemented)
        .RequirePermission(AccountingPermissionConstants.Consumption.Search);
    }
}