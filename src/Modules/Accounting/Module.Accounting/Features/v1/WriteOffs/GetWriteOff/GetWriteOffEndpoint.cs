using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.WriteOffs;
using FSH.Module.Accounting.Contracts.v1.WriteOffs.GetWriteOff;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.WriteOffs.GetWriteOff;

public static class GetWriteOffEndpoint
{
    public static RouteHandlerBuilder MapGetWriteOffEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetWriteOffQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetWriteOffEndpoint))
        .WithSummary("Get WriteOff by ID")
        .Produces<WriteOffDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.WriteOffs.View);
    }
}
