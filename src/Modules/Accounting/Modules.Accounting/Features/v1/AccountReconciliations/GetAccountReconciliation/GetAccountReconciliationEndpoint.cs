using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.AccountReconciliations;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.AccountReconciliations.GetAccountReconciliation;

public static class GetAccountReconciliationEndpoint
{
    public static RouteHandlerBuilder MapGetAccountReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetAccountReconciliationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetAccountReconciliationEndpoint))
        .WithSummary("Get AccountReconciliation by ID")
        .Produces<AccountReconciliationDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.AccountReconciliations.View);
    }
}
