using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.GetApprovalWorkflow;
using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.GetApprovalWorkflow;

public static class GetApprovalWorkflowEndpoint
{
    public static RouteHandlerBuilder MapGetApprovalWorkflowEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetApprovalWorkflowQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetApprovalWorkflowEndpoint))
        .WithSummary("Get ApprovalWorkflow")
        .Produces<ApprovalWorkflowDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalWorkflows.View);
    }
}
