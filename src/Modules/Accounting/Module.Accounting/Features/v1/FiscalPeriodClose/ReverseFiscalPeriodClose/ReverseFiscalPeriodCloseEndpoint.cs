// TODO: Implement Reverse endpoint for FiscalPeriodClose
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.ReverseFiscalPeriodClose;

public static class ReverseFiscalPeriodCloseEndpoint
{
    public static RouteHandlerBuilder MapReverseFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ReverseFiscalPeriodCloseCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ReverseFiscalPeriodCloseEndpoint))
        .WithSummary("Reverse FiscalPeriodClose")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FiscalPeriodClose.Reverse);
    }
}
