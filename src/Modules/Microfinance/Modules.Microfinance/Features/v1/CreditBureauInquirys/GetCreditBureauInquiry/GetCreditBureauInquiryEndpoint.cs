using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CreditBureauInquirys;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CreditBureauInquirys.GetCreditBureauInquiry;

public static class GetCreditBureauInquiryEndpoint
{
    public static RouteHandlerBuilder MapGetCreditBureauInquiryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCreditBureauInquiryQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCreditBureauInquiryEndpoint))
        .WithSummary("Get CreditBureauInquiry")
        .Produces<CreditBureauInquiryDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CreditBureauInquirys.View);
    }
}
