using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.DeleteLegalAction;

public static class DeleteLegalActionEndpoint
{
    public static RouteHandlerBuilder MapDeleteLegalActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteLegalActionCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteLegalActionEndpoint))
        .WithSummary("Delete LegalAction")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.LegalActions.Delete);
    }
}
