using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.GetMemberGroups;
using FSH.Module.Microfinance.Contracts.v1.MemberGroups;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.GetMemberGroups;

public static class GetMemberGroupsEndpoint
{
    public static RouteHandlerBuilder MapGetMemberGroupsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMemberGroupsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMemberGroupsEndpoint))
        .WithSummary("Get MemberGroups")
        .Produces<MemberGroupsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.MemberGroups.Search);
    }
}
