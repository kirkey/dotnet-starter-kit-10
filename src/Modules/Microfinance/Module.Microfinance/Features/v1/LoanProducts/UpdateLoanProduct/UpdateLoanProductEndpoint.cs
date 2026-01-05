using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanProducts.UpdateLoanProduct;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.UpdateLoanProduct;

public static class UpdateLoanProductEndpoint
{
    public static RouteHandlerBuilder MapUpdateLoanProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateLoanProductCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateLoanProductEndpoint))
        .WithSummary("Update LoanProduct")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanProducts.Update);
    }
}
