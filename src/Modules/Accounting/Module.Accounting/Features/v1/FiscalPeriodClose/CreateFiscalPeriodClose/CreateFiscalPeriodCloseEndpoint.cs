using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.FiscalPeriodClose.CreateFiscalPeriodClose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CreateFiscalPeriodClose;

public static class CreateFiscalPeriodCloseEndpoint
{
    public static RouteHandlerBuilder MapCreateFiscalPeriodCloseEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateFiscalPeriodCloseCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateFiscalPeriodCloseEndpoint))
        .WithSummary("Create FiscalPeriodClose")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.FiscalPeriodClose.Create);
    }
}
