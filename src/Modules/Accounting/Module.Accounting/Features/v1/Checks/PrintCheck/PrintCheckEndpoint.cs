// TODO: Implement Print endpoint for Check
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Checks.PrintCheck;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.PrintCheck;

public static class PrintCheckEndpoint
{
    public static RouteHandlerBuilder MapPrintCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new PrintCheckCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(PrintCheckEndpoint))
        .WithSummary("Print Check")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.Print);
    }
}
