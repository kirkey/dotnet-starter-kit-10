using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.InsuranceClaims;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceClaims.GetInsuranceClaim;

public static class GetInsuranceClaimEndpoint
{
    public static RouteHandlerBuilder MapGetInsuranceClaimEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInsuranceClaimQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInsuranceClaimEndpoint))
        .WithSummary("Get InsuranceClaim")
        .Produces<InsuranceClaimDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceClaims.View);
    }
}
