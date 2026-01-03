using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.RiskCategorys.DeleteRiskCategory;

public static class DeleteRiskCategoryEndpoint
{
    public static RouteHandlerBuilder MapDeleteRiskCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteRiskCategoryCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteRiskCategoryEndpoint))
        .WithSummary("Delete RiskCategory")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.RiskCategorys.Delete);
    }
}
