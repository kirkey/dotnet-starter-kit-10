using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentTransactions.CreateInvestmentTransaction;

namespace FSH.Module.Microfinance.Features.v1.InvestmentTransactions.CreateInvestmentTransaction;

public static class CreateInvestmentTransactionEndpoint
{
    public static RouteHandlerBuilder MapCreateInvestmentTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInvestmentTransactionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateInvestmentTransactionEndpoint))
        .WithSummary("Create InvestmentTransaction")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentTransactions.Create);
    }
}
