using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.FeePayments.GetFeePayments;
using FSH.Module.Microfinance.Contracts.v1.FeePayments;

namespace FSH.Module.Microfinance.Features.v1.FeePayments.GetFeePayments;

public static class GetFeePaymentsEndpoint
{
    public static RouteHandlerBuilder MapGetFeePaymentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetFeePaymentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetFeePaymentsEndpoint))
        .WithSummary("Get FeePayments")
        .Produces<FeePaymentsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.FeePayments.Search);
    }
}
