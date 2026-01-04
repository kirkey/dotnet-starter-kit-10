using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.UpdateLoanGuarantor;

public static class UpdateLoanGuarantorEndpoint
{
    public static RouteHandlerBuilder MapUpdateLoanGuarantorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateLoanGuarantorCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateLoanGuarantorEndpoint))
        .WithSummary("Update LoanGuarantor")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanGuarantors.Update);
    }
}
