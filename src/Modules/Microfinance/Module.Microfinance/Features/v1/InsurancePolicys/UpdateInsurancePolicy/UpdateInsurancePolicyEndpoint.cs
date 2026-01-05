using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InsurancePolicys.UpdateInsurancePolicy;

namespace FSH.Module.Microfinance.Features.v1.InsurancePolicys.UpdateInsurancePolicy;

public static class UpdateInsurancePolicyEndpoint
{
    public static RouteHandlerBuilder MapUpdateInsurancePolicyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInsurancePolicyCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateInsurancePolicyEndpoint))
        .WithSummary("Update InsurancePolicy")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InsurancePolicys.Update);
    }
}
