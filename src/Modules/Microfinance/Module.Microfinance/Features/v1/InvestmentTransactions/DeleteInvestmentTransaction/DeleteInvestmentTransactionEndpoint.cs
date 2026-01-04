using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.DeleteInvestmentTransaction;

public static class DeleteInvestmentTransactionEndpoint
{
    public static RouteHandlerBuilder MapDeleteInvestmentTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInvestmentTransactionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInvestmentTransactionEndpoint))
        .WithSummary("Delete InvestmentTransaction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentTransactions.Delete);
    }
}
