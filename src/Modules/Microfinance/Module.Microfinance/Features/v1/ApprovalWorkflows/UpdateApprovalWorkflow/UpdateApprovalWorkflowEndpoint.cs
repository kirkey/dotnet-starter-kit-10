using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.UpdateApprovalWorkflow;

public static class UpdateApprovalWorkflowEndpoint
{
    public static RouteHandlerBuilder MapUpdateApprovalWorkflowEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateApprovalWorkflowCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateApprovalWorkflowEndpoint))
        .WithSummary("Update ApprovalWorkflow")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalWorkflows.Update);
    }
}
