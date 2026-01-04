using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.PatronageCapital;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.GetPatronageCapital;

public static class GetPatronageCapitalEndpoint
{
    public static RouteHandlerBuilder MapGetPatronageCapitalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new GetPatronageCapitalQuery(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(GetPatronageCapitalEndpoint))
        .WithSummary("Get PatronageCapital by ID")
        .Produces<PatronageCapitalDto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PatronageCapital.View);
    }
}
