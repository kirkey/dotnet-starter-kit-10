using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.CreditBureauReports;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauReports.GetCreditBureauReports;

public static class GetCreditBureauReportsEndpoint
{
    public static RouteHandlerBuilder MapGetCreditBureauReportsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditBureauReportsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditBureauReportsEndpoint))
        .WithSummary("Get CreditBureauReports")
        .Produces<CreditBureauReportsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauReports.Search);
    }
}
