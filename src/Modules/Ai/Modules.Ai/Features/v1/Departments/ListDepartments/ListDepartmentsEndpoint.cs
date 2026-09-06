using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Ai.Contracts.Authorization;
using FSH.Modules.Ai.Contracts.v1.Departments;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Ai.Features.v1.Departments.ListDepartments;

public static class ListDepartmentsEndpoint
{
    internal static RouteHandlerBuilder MapListDepartmentsEndpoint(this IEndpointRouteBuilder endpoints) =>
        endpoints.MapGet("/departments",
                async (IMediator mediator, CancellationToken ct) =>
                    Results.Ok(await mediator.Send(new ListDepartmentsQuery(), ct).ConfigureAwait(false)))
            .WithName("ListDepartments")
            .WithSummary("List agent departments")
            .RequirePermission(AiPermissions.Agents.View);
}
