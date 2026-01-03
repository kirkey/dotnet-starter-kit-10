using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.LoanWriteOffs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOff;

public static class GetLoanWriteOffEndpoint
{
    public static RouteHandlerBuilder MapGetLoanWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanWriteOffQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanWriteOffEndpoint))
        .WithSummary("Get LoanWriteOff")
        .Produces<LoanWriteOffDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanWriteOffs.View);
    }
}
