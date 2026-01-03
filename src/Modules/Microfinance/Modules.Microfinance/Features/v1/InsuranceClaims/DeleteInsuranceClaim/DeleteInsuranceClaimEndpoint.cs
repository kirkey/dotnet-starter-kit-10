using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.InsuranceClaims.DeleteInsuranceClaim;

public static class DeleteInsuranceClaimEndpoint
{
    public static RouteHandlerBuilder MapDeleteInsuranceClaimEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInsuranceClaimCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInsuranceClaimEndpoint))
        .WithSummary("Delete InsuranceClaim")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InsuranceClaims.Delete);
    }
}
