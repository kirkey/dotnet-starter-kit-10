using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Accruals.DeleteAccrual;

public static class DeleteAccrualEndpoint
{
    public static RouteHandlerBuilder MapDeleteAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteAccrualCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteAccrualEndpoint))
        .WithSummary("Delete Accrual")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Accruals.Delete);
    }
}
