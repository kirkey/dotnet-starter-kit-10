using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.CollectionCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.GetCollectionCase;

public static class GetCollectionCaseEndpoint
{
    public static RouteHandlerBuilder MapGetCollectionCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollectionCaseQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollectionCaseEndpoint))
        .WithSummary("Get CollectionCase")
        .Produces<CollectionCaseDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionCases.View);
    }
}
