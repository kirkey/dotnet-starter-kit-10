using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.Documents.GetDocuments;
using FSH.Module.Microfinance.Contracts.v1.Documents;

namespace FSH.Module.Microfinance.Features.v1.Documents.GetDocuments;

public static class GetDocumentsEndpoint
{
    public static RouteHandlerBuilder MapGetDocumentsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDocumentsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDocumentsEndpoint))
        .WithSummary("Get Documents")
        .Produces<DocumentsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Documents.Search);
    }
}
