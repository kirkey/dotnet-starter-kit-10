using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.Accruals.CreateAccrual;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.Accruals.CreateAccrual;

public static class CreateAccrualEndpoint
{
    public static RouteHandlerBuilder MapCreateAccrualEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateAccrualCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateAccrualEndpoint))
        .WithSummary("Create Accrual")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.Accruals.Create);
    }
}
