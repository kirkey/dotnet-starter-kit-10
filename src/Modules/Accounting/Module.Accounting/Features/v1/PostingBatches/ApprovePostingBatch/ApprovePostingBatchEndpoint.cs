// TODO: Implement Approve endpoint for PostingBatch
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.ApprovePostingBatch;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.ApprovePostingBatch;

public static class ApprovePostingBatchEndpoint
{
    public static RouteHandlerBuilder MapApprovePostingBatchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApprovePostingBatchCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApprovePostingBatchEndpoint))
        .WithSummary("Approve PostingBatch")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PostingBatches.Approve);
    }
}
