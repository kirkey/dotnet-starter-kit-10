using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InsuranceClaims.CreateInsuranceClaim;

namespace FSH.Module.Microfinance.Features.v1.InsuranceClaims.CreateInsuranceClaim;

public static class CreateInsuranceClaimEndpoint
{
    public static RouteHandlerBuilder MapCreateInsuranceClaimEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInsuranceClaimCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateInsuranceClaimEndpoint))
        .WithSummary("Create InsuranceClaim")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceClaims.Create);
    }
}
