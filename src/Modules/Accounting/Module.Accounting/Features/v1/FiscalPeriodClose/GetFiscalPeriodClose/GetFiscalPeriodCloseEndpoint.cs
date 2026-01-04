using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;

public static class GetFiscalPeriodCloseByIdEndpoint
{
    public static RouteHandlerBuilder MapGetFiscalPeriodCloseByIdEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFiscalPeriodCloseByIdQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFiscalPeriodCloseByIdEndpoint))
        .WithSummary("Get FiscalPeriodClose by ID")
        .Produces<FiscalPeriodCloseDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FiscalPeriodClose.View);
    }
}
