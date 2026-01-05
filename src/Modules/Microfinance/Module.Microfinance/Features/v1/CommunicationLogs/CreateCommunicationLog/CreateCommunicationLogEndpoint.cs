using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.CommunicationLogs.CreateCommunicationLog;

namespace FSH.Module.Microfinance.Features.v1.CommunicationLogs.CreateCommunicationLog;

public static class CreateCommunicationLogEndpoint
{
    public static RouteHandlerBuilder MapCreateCommunicationLogEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCommunicationLogCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateCommunicationLogEndpoint))
        .WithSummary("Create CommunicationLog")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.CommunicationLogs.Create);
    }
}
