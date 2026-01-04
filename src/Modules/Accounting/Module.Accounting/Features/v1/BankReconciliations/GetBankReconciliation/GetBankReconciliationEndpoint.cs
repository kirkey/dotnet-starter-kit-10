using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.GetBankReconciliation;

public static class GetBankReconciliationEndpoint
{
    public static RouteHandlerBuilder MapGetBankReconciliationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBankReconciliationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBankReconciliationEndpoint))
        .WithSummary("Get BankReconciliation by ID")
        .Produces<BankReconciliationDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.View);
    }
}
