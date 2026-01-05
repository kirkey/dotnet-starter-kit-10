using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.GetLoanWriteOffs;
using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOffs;

public static class GetLoanWriteOffsEndpoint
{
    public static RouteHandlerBuilder MapGetLoanWriteOffsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetLoanWriteOffsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetLoanWriteOffsEndpoint))
        .WithSummary("Get LoanWriteOffs")
        .Produces<LoanWriteOffsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.LoanWriteOffs.Search);
    }
}
