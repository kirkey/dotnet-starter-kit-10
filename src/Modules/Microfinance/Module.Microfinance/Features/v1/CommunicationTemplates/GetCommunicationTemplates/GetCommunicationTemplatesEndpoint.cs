using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates.GetCommunicationTemplates;
using FSH.Module.Microfinance.Contracts.v1.CommunicationTemplates;

namespace FSH.Module.Microfinance.Features.v1.CommunicationTemplates.GetCommunicationTemplates;

public static class GetCommunicationTemplatesEndpoint
{
    public static RouteHandlerBuilder MapGetCommunicationTemplatesEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetCommunicationTemplatesQuery(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetCommunicationTemplatesEndpoint))
        .WithSummary("Get CommunicationTemplates")
        .Produces<CommunicationTemplatesPagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationTemplates.Search);
    }
}
