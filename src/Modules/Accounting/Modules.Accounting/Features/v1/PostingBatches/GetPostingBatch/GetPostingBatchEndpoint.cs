using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.PostingBatches;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.PostingBatches.GetPostingBatch;

public static class GetPostingBatchEndpoint
{
    public static RouteHandlerBuilder MapGetPostingBatchEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPostingBatchQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPostingBatchEndpoint))
        .WithSummary("Get PostingBatch by ID")
        .Produces<PostingBatchDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PostingBatches.View);
    }
}
