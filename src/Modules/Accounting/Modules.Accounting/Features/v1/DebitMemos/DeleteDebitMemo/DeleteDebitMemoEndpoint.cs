using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.DebitMemos.DeleteDebitMemo;

public static class DeleteDebitMemoEndpoint
{
    public static RouteHandlerBuilder MapDeleteDebitMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteDebitMemoCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteDebitMemoEndpoint))
        .WithSummary("Delete DebitMemo")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DebitMemos.Delete);
    }
}
