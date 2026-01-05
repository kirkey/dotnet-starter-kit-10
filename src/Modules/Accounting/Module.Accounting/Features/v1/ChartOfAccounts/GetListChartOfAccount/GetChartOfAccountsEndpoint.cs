using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.GetChartOfAccounts;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccounts;

public static class GetChartOfAccountsEndpoint
{
    public static RouteHandlerBuilder MapGetChartOfAccountsEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetChartOfAccountsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetChartOfAccountsEndpoint))
        .WithSummary("Get paginated list of ChartOfAccounts")
        .Produces<ChartOfAccountsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Search);
    }
}
