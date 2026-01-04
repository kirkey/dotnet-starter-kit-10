using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.BillLineItems.DeleteBillLineItem;

public static class DeleteBillLineItemEndpoint
{
    public static RouteHandlerBuilder MapDeleteBillLineItemEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteBillLineItemCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteBillLineItemEndpoint))
        .WithSummary("Delete BillLineItem")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BillLineItems.Delete);
    }
}
