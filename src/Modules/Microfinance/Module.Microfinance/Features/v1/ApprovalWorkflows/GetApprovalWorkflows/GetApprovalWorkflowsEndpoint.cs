using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflows;

public static class GetApprovalWorkflowsEndpoint
{
    public static RouteHandlerBuilder MapGetApprovalWorkflowsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApprovalWorkflowsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetApprovalWorkflowsEndpoint))
        .WithSummary("Get ApprovalWorkflows")
        .Produces<ApprovalWorkflowsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalWorkflows.Search);
    }
}
