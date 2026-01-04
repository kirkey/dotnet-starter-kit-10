using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ShareAccounts.CreateShareAccount;

public static class CreateShareAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateShareAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateShareAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateShareAccountEndpoint))
        .WithSummary("Create ShareAccount")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.ShareAccounts.Create);
    }
}
