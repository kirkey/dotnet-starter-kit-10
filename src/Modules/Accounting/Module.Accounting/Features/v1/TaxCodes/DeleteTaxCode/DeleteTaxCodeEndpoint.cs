using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.TaxCodes.DeleteTaxCode;

public static class DeleteTaxCodeEndpoint
{
    public static RouteHandlerBuilder MapDeleteTaxCodeEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteTaxCodeCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteTaxCodeEndpoint))
        .WithSummary("Delete TaxCode")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.TaxCodes.Delete);
    }
}
