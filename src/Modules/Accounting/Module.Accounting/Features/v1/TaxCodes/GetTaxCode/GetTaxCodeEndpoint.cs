using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.TaxCodes;
using FSH.Module.Accounting.Contracts.v1.TaxCodes.GetTaxCode;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.GetTaxCode;

public static class GetTaxCodeEndpoint
{
    public static RouteHandlerBuilder MapGetTaxCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetTaxCodeQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTaxCodeEndpoint))
        .WithSummary("Get TaxCode by ID")
        .Produces<TaxCodeDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TaxCodes.View);
    }
}
