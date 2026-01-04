using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;

public static class GetRetainedEarningsEndpoint
{
    public static RouteHandlerBuilder MapGetRetainedEarningsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRetainedEarningsQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRetainedEarningsEndpoint))
        .WithSummary("Get RetainedEarnings by ID")
        .Produces<RetainedEarningsDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.View);
    }
}
