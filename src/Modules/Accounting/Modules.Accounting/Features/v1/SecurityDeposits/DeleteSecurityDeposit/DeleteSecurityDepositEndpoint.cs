using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.SecurityDeposits.DeleteSecurityDeposit;

public static class DeleteSecurityDepositEndpoint
{
    public static RouteHandlerBuilder MapDeleteSecurityDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteSecurityDepositCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteSecurityDepositEndpoint))
        .WithSummary("Delete SecurityDeposit")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.Delete);
    }
}
