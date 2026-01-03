using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.UpdateLoanDisbursementTranche;

public static class UpdateLoanDisbursementTrancheEndpoint
{
    public static RouteHandlerBuilder MapUpdateLoanDisbursementTrancheEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateLoanDisbursementTrancheCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateLoanDisbursementTrancheEndpoint))
        .WithSummary("Update LoanDisbursementTranche")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanDisbursementTranches.Update);
    }
}
