using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.LoanApplications;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanApplications.GetLoanApplication;

public static class GetLoanApplicationEndpoint
{
    public static RouteHandlerBuilder MapGetLoanApplicationEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanApplicationQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanApplicationEndpoint))
        .WithSummary("Get LoanApplication")
        .Produces<LoanApplicationDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanApplications.View);
    }
}
