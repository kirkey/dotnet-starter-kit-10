using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Payees.DeletePayee;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payees.DeletePayee;

public static class DeletePayeeEndpoint
{
    public static RouteHandlerBuilder MapDeletePayeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePayeeCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePayeeEndpoint))
        .WithSummary("Delete Payee")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payees.Delete);
    }
}
