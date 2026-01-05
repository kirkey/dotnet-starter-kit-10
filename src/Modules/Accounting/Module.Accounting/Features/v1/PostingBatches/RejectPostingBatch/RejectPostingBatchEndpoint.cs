// TODO: Implement Reject endpoint for PostingBatch
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.RejectPostingBatch;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.RejectPostingBatch;

public static class RejectPostingBatchEndpoint
{
    public static RouteHandlerBuilder MapRejectPostingBatchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new RejectPostingBatchCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(RejectPostingBatchEndpoint))
        .WithSummary("Reject PostingBatch")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PostingBatches.Reject);
    }
}
