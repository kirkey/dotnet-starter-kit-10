using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.Members;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


namespace FSH.Module.Microfinance.Features.v1.Members.GetMember;

public static class GetMemberEndpoint
{
    public static RouteHandlerBuilder MapGetMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var query = new GetMemberQuery(id);
            var result = await mediator.Send(query, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMemberEndpoint))
        .WithSummary("Get member by ID")
        .WithDescription("Retrieves a single member with full details")
        .Produces<MemberDto>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound)
        .RequirePermission(MicrofinancePermissionConstants.Members.View);
    }
}
