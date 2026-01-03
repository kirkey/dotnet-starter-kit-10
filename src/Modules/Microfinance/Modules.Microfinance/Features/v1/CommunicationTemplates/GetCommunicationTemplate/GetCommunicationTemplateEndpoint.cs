using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.CommunicationTemplates;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplate;

public static class GetCommunicationTemplateEndpoint
{
    public static RouteHandlerBuilder MapGetCommunicationTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCommunicationTemplateQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCommunicationTemplateEndpoint))
        .WithSummary("Get CommunicationTemplate")
        .Produces<CommunicationTemplateDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationTemplates.View);
    }
}
