using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.UpdateCreditBureauReport;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.UpdateCreditBureauReport;

public static class UpdateCreditBureauReportEndpoint
{
    public static RouteHandlerBuilder MapUpdateCreditBureauReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCreditBureauReportCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCreditBureauReportEndpoint))
        .WithSummary("Update CreditBureauReport")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauReports.Update);
    }
}
