using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.Payees;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.Payees.GetPayee;

public static class GetPayeeEndpoint
{
    public static RouteHandlerBuilder MapGetPayeeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPayeeQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPayeeEndpoint))
        .WithSummary("Get Payee by ID")
        .Produces<PayeeDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Payees.View);
    }
}
