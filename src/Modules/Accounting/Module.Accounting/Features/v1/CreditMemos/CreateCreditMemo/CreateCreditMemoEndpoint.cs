using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.CreditMemos.CreateCreditMemo;

namespace FSH.Module.Accounting.Features.v1.CreditMemos.CreateCreditMemo;

public static class CreateCreditMemoEndpoint
{
    public static RouteHandlerBuilder MapCreateCreditMemoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateCreditMemoCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateCreditMemoEndpoint))
        .WithSummary("Create CreditMemo")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.CreditMemos.Create);
    }
}
