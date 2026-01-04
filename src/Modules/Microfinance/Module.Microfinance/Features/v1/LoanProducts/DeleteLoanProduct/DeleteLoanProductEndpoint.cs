using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanProducts.DeleteLoanProduct;

public static class DeleteLoanProductEndpoint
{
    public static RouteHandlerBuilder MapDeleteLoanProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLoanProductCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLoanProductEndpoint))
        .WithSummary("Delete LoanProduct")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LoanProducts.Delete);
    }
}
