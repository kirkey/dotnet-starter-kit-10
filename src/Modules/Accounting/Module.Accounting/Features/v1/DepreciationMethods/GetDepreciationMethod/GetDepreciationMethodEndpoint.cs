using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.DepreciationMethods;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.GetDepreciationMethod;

public static class GetDepreciationMethodEndpoint
{
    public static RouteHandlerBuilder MapGetDepreciationMethodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDepreciationMethodQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDepreciationMethodEndpoint))
        .WithSummary("Get DepreciationMethod by ID")
        .Produces<DepreciationMethodDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DepreciationMethods.View);
    }
}
