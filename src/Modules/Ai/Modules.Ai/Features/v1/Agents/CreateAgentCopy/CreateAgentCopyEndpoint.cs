using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.CreateAgentCopy;

public sealed record CreateAgentCopyRequest(string Name);

public static class CreateAgentCopyEndpoint
{
    internal static RouteHandlerBuilder MapCreateAgentCopyEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents/{id:guid}/duplicate",
                async (Guid id, CreateAgentCopyRequest body, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new CreateAgentCopyCommand(id, body.Name), ct).ConfigureAwait(false)))
            .WithName("CreateAgentCopy")
            .WithSummary("Copy a department agent under a new name")
            .RequirePermission(AiPermissions.Agents.Create)
            .WithIdempotency();
}
