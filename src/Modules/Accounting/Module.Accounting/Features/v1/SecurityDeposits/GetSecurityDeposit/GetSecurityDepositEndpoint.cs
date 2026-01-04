using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.SecurityDeposits;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.GetSecurityDeposit;

public static class GetSecurityDepositEndpoint
{
    public static RouteHandlerBuilder MapGetSecurityDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSecurityDepositQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSecurityDepositEndpoint))
        .WithSummary("Get SecurityDeposit by ID")
        .Produces<SecurityDepositDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.View);
    }
}
