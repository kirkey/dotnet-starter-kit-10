using FSH.Framework.Shared.Identity.Authorization;
using FSH.Framework.Web.Idempotency;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Departments;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Departments.CreateDepartment;

public static class CreateDepartmentEndpoint
{
    internal static RouteHandlerBuilder MapCreateDepartmentEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapPost("/departments",
                async (CreateDepartmentCommand command, IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(command, ct).ConfigureAwait(false)))
            .WithName("CreateDepartment")
            .WithSummary("Create an agent department")
            .RequirePermission(AiPermissions.Agents.Create)
            .WithIdempotency();
}
