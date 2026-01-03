using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CustomerSurveys.DeleteCustomerSurvey;

public static class DeleteCustomerSurveyEndpoint
{
    public static RouteHandlerBuilder MapDeleteCustomerSurveyEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCustomerSurveyCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCustomerSurveyEndpoint))
        .WithSummary("Delete CustomerSurvey")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CustomerSurveys.Delete);
    }
}
