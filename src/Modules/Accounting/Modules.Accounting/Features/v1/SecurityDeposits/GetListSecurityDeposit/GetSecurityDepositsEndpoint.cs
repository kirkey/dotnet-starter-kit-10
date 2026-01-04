using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.SecurityDeposits;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.SecurityDeposits.GetSecurityDeposits;

public static class GetSecurityDepositsEndpoint
{
    public static RouteHandlerBuilder MapGetSecurityDepositsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetSecurityDepositsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSecurityDepositsEndpoint))
        .WithSummary("Get paginated list of SecurityDeposits")
        .Produces<SecurityDepositsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.SecurityDeposits.Search);
    }
}
