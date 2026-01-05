using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerCases.UpdateCustomerCase;

namespace FSH.Module.Microfinance.Features.v1.CustomerCases.UpdateCustomerCase;

public static class UpdateCustomerCaseEndpoint
{
    public static RouteHandlerBuilder MapUpdateCustomerCaseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCustomerCaseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCustomerCaseEndpoint))
        .WithSummary("Update CustomerCase")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerCases.Update);
    }
}
