using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LegalActions.GetLegalActions;
using FSH.Module.Microfinance.Contracts.v1.LegalActions;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.GetLegalActions;

public static class GetLegalActionsEndpoint
{
    public static RouteHandlerBuilder MapGetLegalActionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLegalActionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLegalActionsEndpoint))
        .WithSummary("Get LegalActions")
        .Produces<LegalActionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LegalActions.Search);
    }
}
