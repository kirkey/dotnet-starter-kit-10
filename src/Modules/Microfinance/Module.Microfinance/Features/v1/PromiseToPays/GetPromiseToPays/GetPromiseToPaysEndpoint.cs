using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.GetPromiseToPays;
using FSH.Module.Microfinance.Contracts.v1.PromiseToPays;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.GetPromiseToPays;

public static class GetPromiseToPaysEndpoint
{
    public static RouteHandlerBuilder MapGetPromiseToPaysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPromiseToPaysQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPromiseToPaysEndpoint))
        .WithSummary("Get PromiseToPays")
        .Produces<PromiseToPaysPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.PromiseToPays.Search);
    }
}
