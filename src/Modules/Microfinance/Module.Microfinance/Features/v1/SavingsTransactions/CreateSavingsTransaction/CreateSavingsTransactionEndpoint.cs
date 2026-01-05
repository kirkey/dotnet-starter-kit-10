using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.SavingsTransactions.CreateSavingsTransaction;

namespace FSH.Module.Microfinance.Features.v1.SavingsTransactions.CreateSavingsTransaction;

public static class CreateSavingsTransactionEndpoint
{
    public static RouteHandlerBuilder MapCreateSavingsTransactionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateSavingsTransactionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateSavingsTransactionEndpoint))
        .WithSummary("Create SavingsTransaction")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.SavingsTransactions.Create);
    }
}
