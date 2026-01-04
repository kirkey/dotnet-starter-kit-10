using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantor;

public static class GetLoanGuarantorEndpoint
{
    public static RouteHandlerBuilder MapGetLoanGuarantorEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanGuarantorQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanGuarantorEndpoint))
        .WithSummary("Get LoanGuarantor")
        .Produces<LoanGuarantorDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanGuarantors.View);
    }
}
