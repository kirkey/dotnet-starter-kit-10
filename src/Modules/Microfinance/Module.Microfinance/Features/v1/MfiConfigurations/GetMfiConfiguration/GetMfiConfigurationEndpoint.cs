using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.GetMfiConfiguration;

public static class GetMfiConfigurationEndpoint
{
    public static RouteHandlerBuilder MapGetMfiConfigurationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMfiConfigurationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMfiConfigurationEndpoint))
        .WithSummary("Get MfiConfiguration")
        .Produces<MfiConfigurationDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MfiConfigurations.View);
    }
}
