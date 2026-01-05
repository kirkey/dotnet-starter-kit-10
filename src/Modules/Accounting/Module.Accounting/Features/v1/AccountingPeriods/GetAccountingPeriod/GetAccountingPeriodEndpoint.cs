using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.AccountingPeriods;
using FSH.Module.Accounting.Contracts.v1.AccountingPeriods.GetAccountingPeriod;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.AccountingPeriods.GetAccountingPeriod;

public static class GetAccountingPeriodEndpoint
{
    public static RouteHandlerBuilder MapGetAccountingPeriodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccountingPeriodQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAccountingPeriodEndpoint))
        .WithSummary("Get AccountingPeriod by ID")
        .Produces<AccountingPeriodDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountingPeriods.View);
    }
}
