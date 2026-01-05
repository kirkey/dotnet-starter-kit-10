using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.DepreciationMethods.UpdateDepreciationMethod;

namespace FSH.Module.Accounting.Features.v1.DepreciationMethods.UpdateDepreciationMethod;

public static class UpdateDepreciationMethodEndpoint
{
    public static RouteHandlerBuilder MapUpdateDepreciationMethodEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            UpdateDepreciationMethodCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(UpdateDepreciationMethodEndpoint))
        .WithSummary("Update DepreciationMethod")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.DepreciationMethods.Update);
    }
}
