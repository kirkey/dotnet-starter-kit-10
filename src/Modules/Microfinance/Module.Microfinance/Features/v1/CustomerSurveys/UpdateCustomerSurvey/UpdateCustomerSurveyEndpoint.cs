using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CustomerSurveys.UpdateCustomerSurvey;

namespace FSH.Module.Microfinance.Features.v1.CustomerSurveys.UpdateCustomerSurvey;

public static class UpdateCustomerSurveyEndpoint
{
    public static RouteHandlerBuilder MapUpdateCustomerSurveyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCustomerSurveyCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCustomerSurveyEndpoint))
        .WithSummary("Update CustomerSurvey")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSurveys.Update);
    }
}
