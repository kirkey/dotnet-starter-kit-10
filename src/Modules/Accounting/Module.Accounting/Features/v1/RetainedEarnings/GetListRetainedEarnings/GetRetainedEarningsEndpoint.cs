using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.GetListRetainedEarnings;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;

public static class GetRetainedEarningsEndpoint
{
    public static RouteHandlerBuilder MapGetRetainedEarningsEndpoint(this IEndpointRouteBuilder endpoints)
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
                new GetRetainedEarningsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetRetainedEarningsEndpoint))
        .WithSummary("Get paginated list of RetainedEarnings")
        .Produces<RetainedEarningsPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.Search);
    }
}
