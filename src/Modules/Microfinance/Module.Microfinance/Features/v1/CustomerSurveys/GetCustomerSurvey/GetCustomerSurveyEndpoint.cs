using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.GetCustomerSurvey;
using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.GetCustomerSurvey;

public static class GetCustomerSurveyEndpoint
{
    public static RouteHandlerBuilder MapGetCustomerSurveyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCustomerSurveyQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCustomerSurveyEndpoint))
        .WithSummary("Get CustomerSurvey")
        .Produces<CustomerSurveyDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSurveys.View);
    }
}
