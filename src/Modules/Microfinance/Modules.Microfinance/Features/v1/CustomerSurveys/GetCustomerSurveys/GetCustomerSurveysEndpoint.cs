using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CustomerSurveys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurveys;

public static class GetCustomerSurveysEndpoint
{
    public static RouteHandlerBuilder MapGetCustomerSurveysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerSurveysQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCustomerSurveysEndpoint))
        .WithSummary("Get CustomerSurveys")
        .Produces<CustomerSurveysPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSurveys.Search);
    }
}
