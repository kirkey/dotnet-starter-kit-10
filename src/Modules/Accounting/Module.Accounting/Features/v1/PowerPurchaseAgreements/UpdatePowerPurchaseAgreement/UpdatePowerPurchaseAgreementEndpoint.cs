using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.UpdatePowerPurchaseAgreement;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.UpdatePowerPurchaseAgreement;

public static class UpdatePowerPurchaseAgreementEndpoint
{
    public static RouteHandlerBuilder MapUpdatePowerPurchaseAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePowerPurchaseAgreementCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdatePowerPurchaseAgreementEndpoint))
        .WithSummary("Update PowerPurchaseAgreement")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PowerPurchaseAgreements.Update);
    }
}
