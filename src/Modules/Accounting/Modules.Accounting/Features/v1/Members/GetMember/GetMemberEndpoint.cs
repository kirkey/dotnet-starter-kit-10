using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Members;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Members.GetMember;

public static class GetMemberEndpoint
{
    public static RouteHandlerBuilder MapGetMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetMemberQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetMemberEndpoint))
        .WithSummary("Get Member by ID")
        .Produces<MemberDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Members.View);
    }
}
