// TODO: Implement Approve endpoint for DebitMemo
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.DebitMemos.ApproveDebitMemo;

public static class ApproveDebitMemoEndpoint
{
    public static RouteHandlerBuilder MapApproveDebitMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveDebitMemoCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveDebitMemoEndpoint))
        .WithSummary("Approve DebitMemo")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DebitMemos.Approve);
    }
}
