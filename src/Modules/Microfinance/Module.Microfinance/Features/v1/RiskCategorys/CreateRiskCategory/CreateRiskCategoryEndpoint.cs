using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskCategorys.CreateRiskCategory;

namespace FSH.Module.Microfinance.Features.v1.RiskCategorys.CreateRiskCategory;

public static class CreateRiskCategoryEndpoint
{
    public static RouteHandlerBuilder MapCreateRiskCategoryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateRiskCategoryCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateRiskCategoryEndpoint))
        .WithSummary("Create RiskCategory")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.RiskCategorys.Create);
    }
}
