using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.PowerPurchaseAgreements.DeletePowerPurchaseAgreement;

public static class DeletePowerPurchaseAgreementEndpoint
{
    public static RouteHandlerBuilder MapDeletePowerPurchaseAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeletePowerPurchaseAgreementCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeletePowerPurchaseAgreementEndpoint))
        .WithSummary("Delete PowerPurchaseAgreement")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PowerPurchaseAgreements.Delete);
    }
}
