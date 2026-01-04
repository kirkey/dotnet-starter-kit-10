using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Bills;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Bills.GetBill;

public static class GetBillEndpoint
{
    public static RouteHandlerBuilder MapGetBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBillQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBillEndpoint))
        .WithSummary("Get Bill by ID")
        .Produces<BillDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Bills.View);
    }
}
