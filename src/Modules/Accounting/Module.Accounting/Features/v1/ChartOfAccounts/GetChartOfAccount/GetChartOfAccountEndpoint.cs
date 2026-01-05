using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts.GetChartOfAccount;
using FSH.Module.Accounting.Contracts.v1.ChartOfAccounts;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccount;

public static class GetChartOfAccountEndpoint
{
    public static RouteHandlerBuilder MapGetChartOfAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetChartOfAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetChartOfAccountEndpoint))
        .WithSummary("Get ChartOfAccount by ID")
        .Produces<ChartOfAccountDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.View);
    }
}
