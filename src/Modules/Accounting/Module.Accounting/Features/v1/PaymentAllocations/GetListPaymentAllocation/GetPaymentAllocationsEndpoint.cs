using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations;
using FSH.Module.Accounting.Contracts.v1.PaymentAllocations.GetListPaymentAllocation;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocations;

public static class GetPaymentAllocationsEndpoint
{
    public static RouteHandlerBuilder MapGetPaymentAllocationsEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetPaymentAllocationsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPaymentAllocationsEndpoint))
        .WithSummary("Get paginated list of PaymentAllocations")
        .Produces<PaymentAllocationsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PaymentAllocations.Search);
    }
}
