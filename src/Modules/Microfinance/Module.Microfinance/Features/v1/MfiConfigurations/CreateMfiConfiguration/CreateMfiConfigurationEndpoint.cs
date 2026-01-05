using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MfiConfigurations.CreateMfiConfiguration;

namespace FSH.Module.Microfinance.Features.v1.MfiConfigurations.CreateMfiConfiguration;

public static class CreateMfiConfigurationEndpoint
{
    public static RouteHandlerBuilder MapCreateMfiConfigurationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateMfiConfigurationCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateMfiConfigurationEndpoint))
        .WithSummary("Create MfiConfiguration")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.MfiConfigurations.Create);
    }
}
