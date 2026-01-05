using FSH.Module.Store.Contracts.v1.POS;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Store.Features.v1.POS.CreatePOS;

public static class CreatePOSEndpoint
{
    public static RouteHandlerBuilder MapCreatePOSEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/pos", async (
            CreatePOSCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/store/pos/{id}", id);
        })
        .WithName(nameof(CreatePOSEndpoint))
        .WithSummary("Create a new POS terminal")
        .RequirePermission(StorePermissionConstants.POS.Create)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .WithOpenApi();
    }
}
