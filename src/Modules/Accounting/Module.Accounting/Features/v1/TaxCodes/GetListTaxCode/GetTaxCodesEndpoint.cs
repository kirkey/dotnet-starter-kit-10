using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.TaxCodes;
using FSH.Module.Accounting.Contracts.v1.TaxCodes.GetListTaxCode;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.GetListTaxCode;

public static class GetTaxCodesEndpoint
{
    public static RouteHandlerBuilder MapGetTaxCodesEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetTaxCodesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetTaxCodesEndpoint))
        .WithSummary("Get paginated list of TaxCodes")
        .Produces<TaxCodesPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TaxCodes.Search);
    }
}
