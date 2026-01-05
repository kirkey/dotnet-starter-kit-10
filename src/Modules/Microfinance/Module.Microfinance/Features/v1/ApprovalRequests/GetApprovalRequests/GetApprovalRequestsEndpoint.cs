using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests.GetApprovalRequests;
using FSH.Module.Microfinance.Contracts.v1.ApprovalRequests;

namespace FSH.Module.Microfinance.Features.v1.ApprovalRequests.GetApprovalRequests;

public static class GetApprovalRequestsEndpoint
{
    public static RouteHandlerBuilder MapGetApprovalRequestsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApprovalRequestsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetApprovalRequestsEndpoint))
        .WithSummary("Get ApprovalRequests")
        .Produces<ApprovalRequestsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalRequests.Search);
    }
}
