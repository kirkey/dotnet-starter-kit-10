using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.CreateCollectionCase;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.CreateCollectionCase;

public static class CreateCollectionCaseEndpoint
{
    public static RouteHandlerBuilder MapCreateCollectionCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCollectionCaseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCollectionCaseEndpoint))
        .WithSummary("Create CollectionCase")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CollectionCases.Create);
    }
}
