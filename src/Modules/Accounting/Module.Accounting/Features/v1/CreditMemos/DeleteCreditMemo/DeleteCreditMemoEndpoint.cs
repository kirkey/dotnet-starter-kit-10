using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.DeleteCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.DeleteCreditMemo;

public static class DeleteCreditMemoEndpoint
{
    public static RouteHandlerBuilder MapDeleteCreditMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteCreditMemoCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteCreditMemoEndpoint))
        .WithSummary("Delete CreditMemo")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CreditMemos.Delete);
    }
}
