using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.DeleteCommunicationTemplate;

public static class DeleteCommunicationTemplateEndpoint
{
    public static RouteHandlerBuilder MapDeleteCommunicationTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCommunicationTemplateCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCommunicationTemplateEndpoint))
        .WithSummary("Delete CommunicationTemplate")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationTemplates.Delete);
    }
}
