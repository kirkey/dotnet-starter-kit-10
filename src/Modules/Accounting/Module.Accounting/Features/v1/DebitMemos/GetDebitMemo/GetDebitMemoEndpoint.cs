using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.DebitMemos;
using FSH.Module.Accounting.Contracts.v1.DebitMemos.GetDebitMemo;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.DebitMemos.GetDebitMemo;

public static class GetDebitMemoEndpoint
{
    public static RouteHandlerBuilder MapGetDebitMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetDebitMemoQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetDebitMemoEndpoint))
        .WithSummary("Get DebitMemo by ID")
        .Produces<DebitMemoDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DebitMemos.View);
    }
}
