using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Banks;
using FSH.Module.Accounting.Contracts.v1.Banks.GetBank;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Banks.GetBank;

public static class GetBankEndpoint
{
    public static RouteHandlerBuilder MapGetBankEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetBankQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetBankEndpoint))
        .WithSummary("Get Bank by ID")
        .Produces<BankDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Banks.View);
    }
}
