using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.UpdateCommunicationTemplate;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.UpdateCommunicationTemplate;

public static class UpdateCommunicationTemplateEndpoint
{
    public static RouteHandlerBuilder MapUpdateCommunicationTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateCommunicationTemplateCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateCommunicationTemplateEndpoint))
        .WithSummary("Update CommunicationTemplate")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationTemplates.Update);
    }
}
