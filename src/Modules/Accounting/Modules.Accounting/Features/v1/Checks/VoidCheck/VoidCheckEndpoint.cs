// TODO: Implement Void endpoint for Check
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Checks.VoidCheck;

public static class VoidCheckEndpoint
{
    public static RouteHandlerBuilder MapVoidCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new VoidCheckCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(VoidCheckEndpoint))
        .WithSummary("Void Check")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Void);
    }
}
