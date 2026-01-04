using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.CreditMemos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.CreditMemos.GetCreditMemo;

public static class GetCreditMemoEndpoint
{
    public static RouteHandlerBuilder MapGetCreditMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditMemoQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditMemoEndpoint))
        .WithSummary("Get CreditMemo by ID")
        .Produces<CreditMemoDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CreditMemos.View);
    }
}
