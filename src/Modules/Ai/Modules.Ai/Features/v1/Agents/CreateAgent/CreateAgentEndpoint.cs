using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Agents;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Agents.CreateAgent;

public static class CreateAgentEndpoint
{
    internal static RouteHandlerBuilder MapCreateAgentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/agents",
                async (CreateAgentCommand command, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("CreateAgent")
            .WithSummary("Create a department agent")
            .RequirePermission(AiPermissions.Agents.Create)
            .WithIdempotency();
}
