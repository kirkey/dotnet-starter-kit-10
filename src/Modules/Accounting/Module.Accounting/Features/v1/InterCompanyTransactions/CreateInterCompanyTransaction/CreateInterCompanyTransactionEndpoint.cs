using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.InterCompanyTransactions.CreateInterCompanyTransaction;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.InterCompanyTransactions.CreateInterCompanyTransaction;

public static class CreateInterCompanyTransactionEndpoint
{
    public static RouteHandlerBuilder MapCreateInterCompanyTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInterCompanyTransactionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateInterCompanyTransactionEndpoint))
        .WithSummary("Create InterCompanyTransaction")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterCompanyTransactions.Create);
    }
}
