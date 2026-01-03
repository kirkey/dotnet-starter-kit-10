using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.Branches;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Branches.GetBranch;

public static class GetBranchEndpoint
{
    public static RouteHandlerBuilder MapGetBranchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBranchQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBranchEndpoint))
        .WithSummary("Get Branch")
        .Produces<BranchDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Branches.View);
    }
}
