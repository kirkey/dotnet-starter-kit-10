using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinitions;

public static class GetFeeDefinitionsEndpoint
{
    public static RouteHandlerBuilder MapGetFeeDefinitionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeeDefinitionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeeDefinitionsEndpoint))
        .WithSummary("Get FeeDefinitions")
        .Produces<FeeDefinitionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeeDefinitions.Search);
    }
}
