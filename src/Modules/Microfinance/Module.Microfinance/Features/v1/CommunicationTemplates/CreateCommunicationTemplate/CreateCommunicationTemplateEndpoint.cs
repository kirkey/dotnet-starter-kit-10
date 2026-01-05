using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.CreateCommunicationTemplate;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.CreateCommunicationTemplate;

public static class CreateCommunicationTemplateEndpoint
{
    public static RouteHandlerBuilder MapCreateCommunicationTemplateEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCommunicationTemplateCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCommunicationTemplateEndpoint))
        .WithSummary("Create CommunicationTemplate")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationTemplates.Create);
    }
}
