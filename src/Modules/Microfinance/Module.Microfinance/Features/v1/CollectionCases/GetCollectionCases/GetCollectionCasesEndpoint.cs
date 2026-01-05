using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CollectionCases.GetCollectionCases;
using FSH.Module.Microfinance.Contracts.v1.CollectionCases;

namespace FSH.Module.Microfinance.Features.v1.CollectionCases.GetCollectionCases;

public static class GetCollectionCasesEndpoint
{
    public static RouteHandlerBuilder MapGetCollectionCasesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCollectionCasesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCollectionCasesEndpoint))
        .WithSummary("Get CollectionCases")
        .Produces<CollectionCasesPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CollectionCases.Search);
    }
}
