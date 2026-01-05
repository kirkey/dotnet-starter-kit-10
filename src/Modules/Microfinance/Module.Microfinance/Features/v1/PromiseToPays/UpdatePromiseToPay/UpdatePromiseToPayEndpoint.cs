using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.PromiseToPays.UpdatePromiseToPay;

namespace FSH.Module.Microfinance.Features.v1.PromiseToPays.UpdatePromiseToPay;

public static class UpdatePromiseToPayEndpoint
{
    public static RouteHandlerBuilder MapUpdatePromiseToPayEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePromiseToPayCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdatePromiseToPayEndpoint))
        .WithSummary("Update PromiseToPay")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.PromiseToPays.Update);
    }
}
