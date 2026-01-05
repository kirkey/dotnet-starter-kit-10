using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;


using FSH.Module.Microfinance.Contracts.v1.Members;

namespace FSH.Module.Microfinance.Features.v1.Members.UpdateMember;

public static class UpdateMemberEndpoint
{
    public static RouteHandlerBuilder MapUpdateMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateMemberCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updatedCommand = command with { Id = id };
            var result = await mediator.Send(updatedCommand, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateMemberEndpoint))
        .WithSummary("Update member")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Members.Update);
    }
}
