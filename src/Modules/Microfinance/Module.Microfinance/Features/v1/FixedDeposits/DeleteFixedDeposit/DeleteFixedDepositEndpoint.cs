using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.FixedDeposits.DeleteFixedDeposit;

public static class DeleteFixedDepositEndpoint
{
    public static RouteHandlerBuilder MapDeleteFixedDepositEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteFixedDepositCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteFixedDepositEndpoint))
        .WithSummary("Delete FixedDeposit")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.FixedDeposits.Delete);
    }
}
