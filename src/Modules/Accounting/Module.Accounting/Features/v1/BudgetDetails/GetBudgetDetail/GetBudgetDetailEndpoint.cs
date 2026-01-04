using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BudgetDetails;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BudgetDetails.GetBudgetDetail;

public static class GetBudgetDetailEndpoint
{
    public static RouteHandlerBuilder MapGetBudgetDetailEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBudgetDetailQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBudgetDetailEndpoint))
        .WithSummary("Get BudgetDetail by ID")
        .Produces<BudgetDetailDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BudgetDetails.View);
    }
}
