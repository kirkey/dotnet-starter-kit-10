using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions.GetUssdSessions;
using FSH.Module.Microfinance.Contracts.v1.UssdSessions;

namespace FSH.Module.Microfinance.Features.v1.UssdSessions.GetUssdSessions;

public static class GetUssdSessionsEndpoint
{
    public static RouteHandlerBuilder MapGetUssdSessionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetUssdSessionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetUssdSessionsEndpoint))
        .WithSummary("Get UssdSessions")
        .Produces<UssdSessionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.UssdSessions.Search);
    }
}
