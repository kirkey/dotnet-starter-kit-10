// TODO: Implement StopPayment endpoint for Check
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Checks.StopPaymentCheck;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.StopPaymentCheck;

public static class StopPaymentCheckEndpoint
{
    public static RouteHandlerBuilder MapStopPaymentCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new StopPaymentCheckCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(StopPaymentCheckEndpoint))
        .WithSummary("StopPayment Check")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.StopPayment);
    }
}
