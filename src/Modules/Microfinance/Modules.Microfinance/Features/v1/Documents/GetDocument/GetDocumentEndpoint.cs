using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.Documents;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.Documents.GetDocument;

public static class GetDocumentEndpoint
{
    public static RouteHandlerBuilder MapGetDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDocumentQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDocumentEndpoint))
        .WithSummary("Get Document")
        .Produces<DocumentDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.Documents.View);
    }
}
