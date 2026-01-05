using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.Documents.DeleteDocument;

namespace FSH.Module.Microfinance.Features.v1.Documents.DeleteDocument;

public static class DeleteDocumentEndpoint
{
    public static RouteHandlerBuilder MapDeleteDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteDocumentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteDocumentEndpoint))
        .WithSummary("Delete Document")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.Documents.Delete);
    }
}
