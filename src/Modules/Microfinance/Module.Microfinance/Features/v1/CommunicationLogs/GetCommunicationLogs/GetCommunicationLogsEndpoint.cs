using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.GetCommunicationLogs;
using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.GetCommunicationLogs;

public static class GetCommunicationLogsEndpoint
{
    public static RouteHandlerBuilder MapGetCommunicationLogsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCommunicationLogsQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCommunicationLogsEndpoint))
        .WithSummary("Get CommunicationLogs")
        .Produces<CommunicationLogsPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationLogs.Search);
    }
}
