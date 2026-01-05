using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.KycDocuments.DeleteKycDocument;

namespace FSH.Module.Microfinance.Features.v1.KycDocuments.DeleteKycDocument;

public static class DeleteKycDocumentEndpoint
{
    public static RouteHandlerBuilder MapDeleteKycDocumentEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteKycDocumentCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteKycDocumentEndpoint))
        .WithSummary("Delete KycDocument")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.KycDocuments.Delete);
    }
}
