using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Accruals;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.Accruals.GetAccrual;

namespace FSH.Module.Accounting.Features.v1.Accruals.GetAccrual;

public static class GetAccrualEndpoint
{
    public static RouteHandlerBuilder MapGetAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccrualQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAccrualEndpoint))
        .WithSummary("Get Accrual by ID")
        .Produces<AccrualDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Accruals.View);
    }
}
