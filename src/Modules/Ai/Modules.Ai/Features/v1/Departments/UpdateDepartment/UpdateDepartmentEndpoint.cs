using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Departments;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Departments.UpdateDepartment;

public static class UpdateDepartmentEndpoint
{
    internal static RouteHandlerBuilder MapUpdateDepartmentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPut("/departments/{id:guid}",
                async (Guid id, UpdateDepartmentCommand command, IMediator mediator, CancellationToken ct) =>
                    id != command.Id
                        ? Results.BadRequest(new { error = "Route id does not match body id." })
                        : Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("UpdateDepartment")
            .WithSummary("Update an agent department")
            .RequirePermission(AiPermissions.Agents.Update);
}
