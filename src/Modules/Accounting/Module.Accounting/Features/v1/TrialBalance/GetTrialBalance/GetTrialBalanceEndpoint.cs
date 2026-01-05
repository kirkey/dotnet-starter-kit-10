using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.TrialBalance;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.TrialBalance.GetTrialBalance;

namespace FSH.Module.Accounting.Features.v1.TrialBalance.GetTrialBalance;

public static class GetTrialBalanceEndpoint
{
    public static RouteHandlerBuilder MapGetTrialBalanceEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTrialBalanceQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTrialBalanceEndpoint))
        .WithSummary("Get TrialBalance by ID")
        .Produces<TrialBalanceDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TrialBalance.View);
    }
}
