using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations.GetListBankReconciliation;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.GetBankReconciliations;

public static class GetBankReconciliationsEndpoint
{
    public static RouteHandlerBuilder MapGetBankReconciliationsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            Guid? bankAccountId,
            string? status,
            DateTime? fromDate,
            DateTime? toDate,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetBankReconciliationsQuery(page, pageSize, bankAccountId, status, fromDate, toDate, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBankReconciliationsEndpoint))
        .WithSummary("Get paginated list of BankReconciliations")
        .Produces<BankReconciliationsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.BankReconciliations.Search);
    }
}
