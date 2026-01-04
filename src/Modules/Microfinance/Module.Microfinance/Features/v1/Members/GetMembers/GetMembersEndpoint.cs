using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.Members;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.Members.GetMembers;

public static class GetMembersEndpoint
{
    public static RouteHandlerBuilder MapGetMembersEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var query = new GetMembersQuery(page, pageSize, searchTerm, isActive);
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMembersEndpoint))
        .WithSummary("Get paginated list of members")
        .WithDescription("Retrieves members with optional filtering")
        .Produces<MembersPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Members.Search);
    }
}
