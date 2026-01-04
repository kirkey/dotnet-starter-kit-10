using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.DeleteInterCompanyTransaction;

public static class DeleteInterCompanyTransactionEndpoint
{
    public static RouteHandlerBuilder MapDeleteInterCompanyTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInterCompanyTransactionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInterCompanyTransactionEndpoint))
        .WithSummary("Delete InterCompanyTransaction")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterCompanyTransactions.Delete);
    }
}
