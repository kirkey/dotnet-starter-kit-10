using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.Members.CreateMember;

public static class CreateMemberEndpoint
{
    public static RouteHandlerBuilder MapCreateMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateMemberCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/members/{id}", id);
        })
        .WithName(nameof(CreateMemberEndpoint))
        .WithSummary("Create a new member")
        .WithDescription("Registers a new member in the microfinance system")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.Members.Create);
    }
}
