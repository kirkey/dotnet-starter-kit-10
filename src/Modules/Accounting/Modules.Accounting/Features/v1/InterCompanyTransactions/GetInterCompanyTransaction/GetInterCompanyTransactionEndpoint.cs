using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.InterCompanyTransactions;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.InterCompanyTransactions.GetInterCompanyTransaction;

public static class GetInterCompanyTransactionEndpoint
{
    public static RouteHandlerBuilder MapGetInterCompanyTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetInterCompanyTransactionQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetInterCompanyTransactionEndpoint))
        .WithSummary("Get InterCompanyTransaction by ID")
        .Produces<InterCompanyTransactionDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.InterCompanyTransactions.View);
    }
}
