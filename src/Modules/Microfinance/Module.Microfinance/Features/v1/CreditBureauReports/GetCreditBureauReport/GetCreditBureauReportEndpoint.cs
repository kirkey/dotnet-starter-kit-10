using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports.GetCreditBureauReport;
using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReport;

public static class GetCreditBureauReportEndpoint
{
    public static RouteHandlerBuilder MapGetCreditBureauReportEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditBureauReportQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditBureauReportEndpoint))
        .WithSummary("Get CreditBureauReport")
        .Produces<CreditBureauReportDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauReports.View);
    }
}
