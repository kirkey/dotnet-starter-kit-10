using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.InvestmentAccounts.DeleteInvestmentAccount;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.DeleteInvestmentAccount;

public static class DeleteInvestmentAccountEndpoint
{
    public static RouteHandlerBuilder MapDeleteInvestmentAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInvestmentAccountCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInvestmentAccountEndpoint))
        .WithSummary("Delete InvestmentAccount")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentAccounts.Delete);
    }
}
