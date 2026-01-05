using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.GetSavingsTransactions;
using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.GetSavingsTransactions;

public static class GetSavingsTransactionsEndpoint
{
    public static RouteHandlerBuilder MapGetSavingsTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSavingsTransactionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSavingsTransactionsEndpoint))
        .WithSummary("Get SavingsTransactions")
        .Produces<SavingsTransactionsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsTransactions.Search);
    }
}
