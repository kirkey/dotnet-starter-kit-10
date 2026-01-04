using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LegalActions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.GetLegalAction;

public static class GetLegalActionEndpoint
{
    public static RouteHandlerBuilder MapGetLegalActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLegalActionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLegalActionEndpoint))
        .WithSummary("Get LegalAction")
        .Produces<LegalActionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LegalActions.View);
    }
}
