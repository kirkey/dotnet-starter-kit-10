using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.SecurityDeposits.CreateSecurityDeposit;

namespace FSH.Module.Accounting.Features.v1.SecurityDeposits.CreateSecurityDeposit;

public static class CreateSecurityDepositEndpoint
{
    public static RouteHandlerBuilder MapCreateSecurityDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateSecurityDepositCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateSecurityDepositEndpoint))
        .WithSummary("Create SecurityDeposit")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.Create);
    }
}
