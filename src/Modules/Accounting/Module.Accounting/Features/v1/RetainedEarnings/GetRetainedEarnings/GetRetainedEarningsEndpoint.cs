using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.GetRetainedEarnings;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;

public static class GetRetainedEarningsByIdEndpoint
{
    public static RouteHandlerBuilder MapGetRetainedEarningsByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetRetainedEarningsByIdQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRetainedEarningsByIdEndpoint))
        .WithSummary("Get RetainedEarnings by ID")
        .Produces<RetainedEarningsDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.View);
    }
}
