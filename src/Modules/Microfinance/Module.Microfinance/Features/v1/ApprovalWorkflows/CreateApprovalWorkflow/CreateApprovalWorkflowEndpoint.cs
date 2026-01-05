using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Microfinance.Contracts.v1.ApprovalWorkflows.CreateApprovalWorkflow;

namespace FSH.Module.Microfinance.Features.v1.ApprovalWorkflows.CreateApprovalWorkflow;

public static class CreateApprovalWorkflowEndpoint
{
    public static RouteHandlerBuilder MapCreateApprovalWorkflowEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateApprovalWorkflowCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateApprovalWorkflowEndpoint))
        .WithSummary("Create ApprovalWorkflow")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.ApprovalWorkflows.Create);
    }
}
