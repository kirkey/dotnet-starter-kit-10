using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.GetInsuranceClaims;
using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaims;

public static class GetInsuranceClaimsEndpoint
{
    public static RouteHandlerBuilder MapGetInsuranceClaimsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInsuranceClaimsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInsuranceClaimsEndpoint))
        .WithSummary("Get InsuranceClaims")
        .Produces<InsuranceClaimsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceClaims.Search);
    }
}
