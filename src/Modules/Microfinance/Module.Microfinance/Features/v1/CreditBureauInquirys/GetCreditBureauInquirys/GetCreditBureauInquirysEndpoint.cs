using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys.GetCreditBureauInquirys;
using FSH.Module.Microfinance.Contracts.v1.CreditBureauInquirys;

namespace FSH.Module.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquirys;

public static class GetCreditBureauInquirysEndpoint
{
    public static RouteHandlerBuilder MapGetCreditBureauInquirysEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditBureauInquirysQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditBureauInquirysEndpoint))
        .WithSummary("Get CreditBureauInquirys")
        .Produces<CreditBureauInquirysPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauInquirys.Search);
    }
}
