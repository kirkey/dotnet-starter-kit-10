using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.GetFeeDefinition;
using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.GetFeeDefinition;

public static class GetFeeDefinitionEndpoint
{
    public static RouteHandlerBuilder MapGetFeeDefinitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeeDefinitionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeeDefinitionEndpoint))
        .WithSummary("Get FeeDefinition")
        .Produces<FeeDefinitionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeeDefinitions.View);
    }
}
