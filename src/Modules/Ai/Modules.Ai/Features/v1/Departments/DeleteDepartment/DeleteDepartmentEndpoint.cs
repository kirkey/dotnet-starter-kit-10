using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Departments;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Departments.DeleteDepartment;

public static class DeleteDepartmentEndpoint
{
    internal static RouteHandlerBuilder MapDeleteDepartmentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapDelete("/departments/{id:guid}",
                async (Guid id, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new DeleteDepartmentCommand(id), ct).ConfigureAwait(false)))
            .WithName("DeleteDepartment")
            .WithSummary("Delete an empty agent department")
            .RequirePermission(AiPermissions.Agents.Delete);
}
