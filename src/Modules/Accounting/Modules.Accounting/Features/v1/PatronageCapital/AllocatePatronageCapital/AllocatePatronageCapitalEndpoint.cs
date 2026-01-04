// TODO: Implement Allocate endpoint for PatronageCapital
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.PatronageCapital.AllocatePatronageCapital;

public static class AllocatePatronageCapitalEndpoint
{
    public static RouteHandlerBuilder MapAllocatePatronageCapitalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new AllocatePatronageCapitalCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(AllocatePatronageCapitalEndpoint))
        .WithSummary("Allocate PatronageCapital")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PatronageCapital.Allocate);
    }
}
