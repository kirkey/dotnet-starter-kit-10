using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.MemberGroups;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.MemberGroups.GetMemberGroup;

public static class GetMemberGroupEndpoint
{
    public static RouteHandlerBuilder MapGetMemberGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMemberGroupQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMemberGroupEndpoint))
        .WithSummary("Get MemberGroup")
        .Produces<MemberGroupDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MemberGroups.View);
    }
}
