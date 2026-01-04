using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.CreateMemberGroup;

public static class CreateMemberGroupEndpoint
{
    public static RouteHandlerBuilder MapCreateMemberGroupEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateMemberGroupCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateMemberGroupEndpoint))
        .WithSummary("Create MemberGroup")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.MemberGroups.Create);
    }
}
