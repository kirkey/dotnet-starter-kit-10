using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Sources;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Sources.AddWebSource;

public static class AddWebSourceEndpoint
{
    internal static RouteHandlerBuilder MapAddWebSourceEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/sources/web",
                async (AddWebSourceCommand command, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("AddWebSource")
            .WithSummary("Add a web link as a knowledge source")
            .RequirePermission(AiPermissions.Sources.Create)
            .WithIdempotency();
}
