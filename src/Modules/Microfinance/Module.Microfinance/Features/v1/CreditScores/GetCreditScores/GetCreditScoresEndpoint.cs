using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CreditScores.GetCreditScores;
using FSH.Module.Microfinance.Contracts.v1.CreditScores;

namespace FSH.Module.Microfinance.Features.v1.CreditScores.GetCreditScores;

public static class GetCreditScoresEndpoint
{
    public static RouteHandlerBuilder MapGetCreditScoresEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditScoresQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditScoresEndpoint))
        .WithSummary("Get CreditScores")
        .Produces<CreditScoresPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditScores.Search);
    }
}
