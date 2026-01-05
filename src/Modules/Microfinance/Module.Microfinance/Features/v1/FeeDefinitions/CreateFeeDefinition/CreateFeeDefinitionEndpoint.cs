using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeeDefinitions.CreateFeeDefinition;

namespace FSH.Module.Microfinance.Features.v1.FeeDefinitions.CreateFeeDefinition;

public static class CreateFeeDefinitionEndpoint
{
    public static RouteHandlerBuilder MapCreateFeeDefinitionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateFeeDefinitionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateFeeDefinitionEndpoint))
        .WithSummary("Create FeeDefinition")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.FeeDefinitions.Create);
    }
}
