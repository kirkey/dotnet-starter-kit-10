using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Shared.Persistence;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.Dtos;
using FSH.Modules.Ai.Contracts.v1.Sources;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Sources.ListSources;

public static class ListSourcesEndpoint
{
    internal static RouteHandlerBuilder MapListSourcesEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/sources",
                async (
                    int? pageNumber, int? pageSize, string? sort,
                    AiSourceKind? kind, AiSourceStatus? status, string? search,
                    IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListSourcesQuery
                    {
                        PageNumber = pageNumber,
                        PageSize = pageSize,
                        Sort = sort,
                        Kind = kind,
                        Status = status,
                        Search = search
                    }, ct).ConfigureAwait(false)))
            .WithName("ListSources")
            .WithSummary("List knowledge sources")
            .RequirePermission(AiPermissions.Sources.View);
}
