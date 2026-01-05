using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.RiskAlerts.CreateRiskAlert;

namespace FSH.Module.Microfinance.Features.v1.RiskAlerts.CreateRiskAlert;

public static class CreateRiskAlertEndpoint
{
    public static RouteHandlerBuilder MapCreateRiskAlertEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateRiskAlertCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateRiskAlertEndpoint))
        .WithSummary("Create RiskAlert")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.RiskAlerts.Create);
    }
}
