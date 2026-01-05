using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLog;
using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLog;

public static class GetCommunicationLogEndpoint
{
    public static RouteHandlerBuilder MapGetCommunicationLogEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCommunicationLogQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCommunicationLogEndpoint))
        .WithSummary("Get CommunicationLog")
        .Produces<CommunicationLogDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationLogs.View);
    }
}
