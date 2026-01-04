using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;

public static class CreatePowerPurchaseAgreementEndpoint
{
    public static RouteHandlerBuilder MapCreatePowerPurchaseAgreementEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreatePowerPurchaseAgreementCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/powerpurchaseagreements/{id}", id);
        })
        .WithName(nameof(CreatePowerPurchaseAgreementEndpoint))
        .WithSummary("Create PowerPurchaseAgreement")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PowerPurchaseAgreements.Create);
    }
}
