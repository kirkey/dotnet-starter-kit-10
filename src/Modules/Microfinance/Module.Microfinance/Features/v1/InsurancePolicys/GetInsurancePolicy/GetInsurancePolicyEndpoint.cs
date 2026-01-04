using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.GetInsurancePolicy;

public static class GetInsurancePolicyEndpoint
{
    public static RouteHandlerBuilder MapGetInsurancePolicyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInsurancePolicyQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInsurancePolicyEndpoint))
        .WithSummary("Get InsurancePolicy")
        .Produces<InsurancePolicyDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InsurancePolicys.View);
    }
}
