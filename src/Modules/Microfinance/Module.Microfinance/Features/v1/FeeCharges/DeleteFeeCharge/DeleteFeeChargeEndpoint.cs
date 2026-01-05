using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeeCharges.DeleteFeeCharge;

namespace FSH.Module.Microfinance.Features.v1.FeeCharges.DeleteFeeCharge;

public static class DeleteFeeChargeEndpoint
{
    public static RouteHandlerBuilder MapDeleteFeeChargeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFeeChargeCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFeeChargeEndpoint))
        .WithSummary("Delete FeeCharge")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.FeeCharges.Delete);
    }
}
