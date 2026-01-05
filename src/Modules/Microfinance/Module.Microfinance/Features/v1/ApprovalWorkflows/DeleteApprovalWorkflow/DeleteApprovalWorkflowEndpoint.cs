using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.DeleteApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.DeleteApprovalWorkflow;

public static class DeleteApprovalWorkflowEndpoint
{
    public static RouteHandlerBuilder MapDeleteApprovalWorkflowEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteApprovalWorkflowCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteApprovalWorkflowEndpoint))
        .WithSummary("Delete ApprovalWorkflow")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalWorkflows.Delete);
    }
}
