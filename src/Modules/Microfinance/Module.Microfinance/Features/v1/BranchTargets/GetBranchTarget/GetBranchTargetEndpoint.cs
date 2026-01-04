using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.BranchTargets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.BranchTargets.GetBranchTarget;

public static class GetBranchTargetEndpoint
{
    public static RouteHandlerBuilder MapGetBranchTargetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBranchTargetQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBranchTargetEndpoint))
        .WithSummary("Get BranchTarget")
        .Produces<BranchTargetDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.BranchTargets.View);
    }
}
