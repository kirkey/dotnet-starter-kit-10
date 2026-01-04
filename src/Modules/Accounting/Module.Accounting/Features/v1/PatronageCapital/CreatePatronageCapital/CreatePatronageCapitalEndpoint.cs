using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.PatronageCapital.CreatePatronageCapital;

public static class CreatePatronageCapitalEndpoint
{
    public static RouteHandlerBuilder MapCreatePatronageCapitalEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreatePatronageCapitalCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/patronagecapital/{id}", id);
        })
        .WithName(nameof(CreatePatronageCapitalEndpoint))
        .WithSummary("Create PatronageCapital")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.PatronageCapital.Create);
    }
}
