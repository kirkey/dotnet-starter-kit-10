using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.GetInterCompanyTransactions;

public static class GetInterCompanyTransactionsEndpoint
{
    public static RouteHandlerBuilder MapGetInterCompanyTransactionsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetInterCompanyTransactionsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInterCompanyTransactionsEndpoint))
        .WithSummary("Get paginated list of InterCompanyTransactions")
        .Produces<InterCompanyTransactionsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterCompanyTransactions.Search);
    }
}
