using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.LoanRestructures;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanRestructures.GetLoanRestructure;

public static class GetLoanRestructureEndpoint
{
    public static RouteHandlerBuilder MapGetLoanRestructureEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanRestructureQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanRestructureEndpoint))
        .WithSummary("Get LoanRestructure")
        .Produces<LoanRestructureDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanRestructures.View);
    }
}
