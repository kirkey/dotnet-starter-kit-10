using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.LoanOfficerTargets;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanOfficerTargets.GetLoanOfficerTarget;

public static class GetLoanOfficerTargetEndpoint
{
    public static RouteHandlerBuilder MapGetLoanOfficerTargetEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanOfficerTargetQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanOfficerTargetEndpoint))
        .WithSummary("Get LoanOfficerTarget")
        .Produces<LoanOfficerTargetDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanOfficerTargets.View);
    }
}
