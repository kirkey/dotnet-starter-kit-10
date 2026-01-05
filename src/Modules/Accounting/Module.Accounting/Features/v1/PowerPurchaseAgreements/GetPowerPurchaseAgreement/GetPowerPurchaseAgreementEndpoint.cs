using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreement;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreement;

public static class GetPowerPurchaseAgreementEndpoint
{
    public static RouteHandlerBuilder MapGetPowerPurchaseAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPowerPurchaseAgreementQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPowerPurchaseAgreementEndpoint))
        .WithSummary("Get PowerPurchaseAgreement by ID")
        .Produces<PowerPurchaseAgreementDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PowerPurchaseAgreements.View);
    }
}
