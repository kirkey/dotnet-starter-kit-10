using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantor;
using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

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
