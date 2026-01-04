using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Checks;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Checks.GetCheck;

public static class GetCheckEndpoint
{
    public static RouteHandlerBuilder MapGetCheckEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCheckQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCheckEndpoint))
        .WithSummary("Get Check by ID")
        .Produces<CheckDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Checks.View);
    }
}
