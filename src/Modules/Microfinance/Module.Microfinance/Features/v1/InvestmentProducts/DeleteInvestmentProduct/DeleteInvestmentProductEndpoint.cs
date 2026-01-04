using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Microfinance.Features.v1.InvestmentProducts.DeleteInvestmentProduct;

public static class DeleteInvestmentProductEndpoint
{
    public static RouteHandlerBuilder MapDeleteInvestmentProductEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new DeleteInvestmentProductCommand(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(DeleteInvestmentProductEndpoint))
        .WithSummary("Delete InvestmentProduct")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.InvestmentProducts.Delete);
    }
}
