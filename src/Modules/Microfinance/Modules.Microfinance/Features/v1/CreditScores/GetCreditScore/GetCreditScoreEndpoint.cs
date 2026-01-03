using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CreditScores;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CreditScores.GetCreditScore;

public static class GetCreditScoreEndpoint
{
    public static RouteHandlerBuilder MapGetCreditScoreEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditScoreQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditScoreEndpoint))
        .WithSummary("Get CreditScore")
        .Produces<CreditScoreDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditScores.View);
    }
}
