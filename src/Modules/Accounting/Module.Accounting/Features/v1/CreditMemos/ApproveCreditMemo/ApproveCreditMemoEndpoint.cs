// TODO: Implement Approve endpoint for CreditMemo
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.ApproveCreditMemo;

public static class ApproveCreditMemoEndpoint
{
    public static RouteHandlerBuilder MapApproveCreditMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ApproveCreditMemoCommand(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(ApproveCreditMemoEndpoint))
        .WithSummary("Approve CreditMemo")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CreditMemos.Approve);
    }
}
