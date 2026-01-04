using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PowerPurchaseAgreements;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreements;

public static class GetPowerPurchaseAgreementsEndpoint
{
    public static RouteHandlerBuilder MapGetPowerPurchaseAgreementsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetPowerPurchaseAgreementsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPowerPurchaseAgreementsEndpoint))
        .WithSummary("Get paginated list of PowerPurchaseAgreements")
        .Produces<PowerPurchaseAgreementsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PowerPurchaseAgreements.Search);
    }
}
