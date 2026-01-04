// TODO: Implement Initiate endpoint for FiscalPeriodClose
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;

public static class InitiateFiscalPeriodCloseEndpoint
{
    public static RouteHandlerBuilder MapInitiateFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new InitiateFiscalPeriodCloseCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(InitiateFiscalPeriodCloseEndpoint))
        .WithSummary("Initiate FiscalPeriodClose")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FiscalPeriodClose.Initiate);
    }
}
