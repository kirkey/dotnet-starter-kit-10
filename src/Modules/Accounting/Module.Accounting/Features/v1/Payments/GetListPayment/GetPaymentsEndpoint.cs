using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Payments;
using FSH.Module.Accounting.Contracts.v1.Payments.GetListPayment;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Payments.GetPayments;

public static class GetPaymentsEndpoint
{
    public static RouteHandlerBuilder MapGetPaymentsEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetPaymentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPaymentsEndpoint))
        .WithSummary("Get paginated list of Payments")
        .Produces<PaymentsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payments.Search);
    }
}
