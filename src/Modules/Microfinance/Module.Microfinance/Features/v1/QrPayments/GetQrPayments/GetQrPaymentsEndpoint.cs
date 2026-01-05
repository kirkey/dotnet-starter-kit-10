using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.QrPayments.GetQrPayments;
using FSH.Module.Microfinance.Contracts.v1.QrPayments;

namespace FSH.Module.Microfinance.Features.v1.QrPayments.GetQrPayments;

public static class GetQrPaymentsEndpoint
{
    public static RouteHandlerBuilder MapGetQrPaymentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetQrPaymentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetQrPaymentsEndpoint))
        .WithSummary("Get QrPayments")
        .Produces<QrPaymentsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.QrPayments.Search);
    }
}
