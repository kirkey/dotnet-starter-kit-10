// TODO: Implement Post endpoint for PostingBatch
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.PostPostingBatch;

public static class PostPostingBatchEndpoint
{
    public static RouteHandlerBuilder MapPostPostingBatchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new PostPostingBatchCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(PostPostingBatchEndpoint))
        .WithSummary("Post PostingBatch")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PostingBatches.Post);
    }
}
