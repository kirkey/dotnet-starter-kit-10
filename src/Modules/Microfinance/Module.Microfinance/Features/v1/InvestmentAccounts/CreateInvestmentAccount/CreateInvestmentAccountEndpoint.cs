using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InvestmentAccounts.CreateInvestmentAccount;

public static class CreateInvestmentAccountEndpoint
{
    public static RouteHandlerBuilder MapCreateInvestmentAccountEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateInvestmentAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateInvestmentAccountEndpoint))
        .WithSummary("Create InvestmentAccount")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentAccounts.Create);
    }
}
