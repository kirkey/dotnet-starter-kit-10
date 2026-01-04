using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.UpdateInsuranceClaim;

public static class UpdateInsuranceClaimEndpoint
{
    public static RouteHandlerBuilder MapUpdateInsuranceClaimEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateInsuranceClaimCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateInsuranceClaimEndpoint))
        .WithSummary("Update InsuranceClaim")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceClaims.Update);
    }
}
