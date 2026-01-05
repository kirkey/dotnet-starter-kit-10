using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.Bills.DeleteBill;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Bills.DeleteBill;

public static class DeleteBillEndpoint
{
    public static RouteHandlerBuilder MapDeleteBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBillCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBillEndpoint))
        .WithSummary("Delete Bill")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Bills.Delete);
    }
}
