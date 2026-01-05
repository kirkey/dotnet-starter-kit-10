using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.PatronageCapital.UpdatePatronageCapital;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.UpdatePatronageCapital;

public static class UpdatePatronageCapitalEndpoint
{
    public static RouteHandlerBuilder MapUpdatePatronageCapitalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdatePatronageCapitalCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdatePatronageCapitalEndpoint))
        .WithSummary("Update PatronageCapital")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PatronageCapital.Update);
    }
}
