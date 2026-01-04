// TODO: Implement Approve endpoint for Bill
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Bills.ApproveBill;

public static class ApproveBillEndpoint
{
    public static RouteHandlerBuilder MapApproveBillEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveBillCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveBillEndpoint))
        .WithSummary("Approve Bill")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Bills.Approve);
    }
}
