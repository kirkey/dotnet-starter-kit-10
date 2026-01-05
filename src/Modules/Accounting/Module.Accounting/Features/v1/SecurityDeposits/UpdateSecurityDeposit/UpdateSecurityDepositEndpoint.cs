using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.SecurityDeposits.UpdateSecurityDeposit;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.UpdateSecurityDeposit;

public static class UpdateSecurityDepositEndpoint
{
    public static RouteHandlerBuilder MapUpdateSecurityDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateSecurityDepositCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateSecurityDepositEndpoint))
        .WithSummary("Update SecurityDeposit")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.Update);
    }
}
