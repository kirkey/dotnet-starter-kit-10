using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using FSH.Module.Microfinance.Contracts.v1.LegalActions.CreateLegalAction;

namespace FSH.Module.Microfinance.Features.v1.LegalActions.CreateLegalAction;

public static class CreateLegalActionEndpoint
{
    public static RouteHandlerBuilder MapCreateLegalActionEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateLegalActionCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/s/{id}", id);
        })
        .WithName(nameof(CreateLegalActionEndpoint))
        .WithSummary("Create LegalAction")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.LegalActions.Create);
    }
}
