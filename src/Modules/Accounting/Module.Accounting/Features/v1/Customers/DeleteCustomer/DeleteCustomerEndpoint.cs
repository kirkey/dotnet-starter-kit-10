using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Customers.DeleteCustomer;

public static class DeleteCustomerEndpoint
{
    public static RouteHandlerBuilder MapDeleteCustomerEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCustomerCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCustomerEndpoint))
        .WithSummary("Delete Customer")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Customers.Delete);
    }
}
