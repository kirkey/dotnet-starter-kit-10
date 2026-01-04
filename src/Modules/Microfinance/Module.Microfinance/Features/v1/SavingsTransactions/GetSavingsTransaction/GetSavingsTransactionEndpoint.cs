using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransaction;

public static class GetSavingsTransactionEndpoint
{
    public static RouteHandlerBuilder MapGetSavingsTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSavingsTransactionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSavingsTransactionEndpoint))
        .WithSummary("Get SavingsTransaction")
        .Produces<SavingsTransactionDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsTransactions.View);
    }
}
