using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Microfinance.Contracts.v1.SavingsAccounts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.SavingsAccounts.GetSavingsAccount;

public static class GetSavingsAccountEndpoint
{
    public static RouteHandlerBuilder MapGetSavingsAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetSavingsAccountQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetSavingsAccountEndpoint))
        .WithSummary("Get SavingsAccount")
        .Produces<SavingsAccountDto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.SavingsAccounts.View);
    }
}
