using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PostingBatches;
using FSH.Module.Accounting.Contracts.v1.PostingBatches.GetListPostingBatch;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PostingBatches.GetListPostingBatch;

public static class GetPostingBatchesEndpoint
{
    public static RouteHandlerBuilder MapGetPostingBatchesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new GetPostingBatchesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPostingBatchesEndpoint))
        .WithSummary("Get paginated list of PostingBatches")
        .Produces<PostingBatchesPagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PostingBatches.Search);
    }
}
