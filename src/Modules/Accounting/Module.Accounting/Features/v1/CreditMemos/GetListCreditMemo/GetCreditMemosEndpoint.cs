using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.CreditMemos;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.GetListCreditMemo;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.GetCreditMemos;

public static class GetCreditMemosEndpoint
{
    public static RouteHandlerBuilder MapGetCreditMemosEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetCreditMemosQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditMemosEndpoint))
        .WithSummary("Get paginated list of CreditMemos")
        .Produces<CreditMemosPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CreditMemos.Search);
    }
}
