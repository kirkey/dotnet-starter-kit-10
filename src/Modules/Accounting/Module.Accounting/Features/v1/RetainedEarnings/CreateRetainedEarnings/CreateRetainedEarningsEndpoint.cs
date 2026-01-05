using FSH.Framework.Shared.Identity.Authorization;
using FSH.Module.Accounting.Contracts.v1.RetainedEarnings.CreateRetainedEarnings;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing; 

namespace FSH.Module.Accounting.Features.v1.RetainedEarnings.CreateRetainedEarnings;

public static class CreateRetainedEarningsEndpoint
{
    public static RouteHandlerBuilder MapCreateRetainedEarningsEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateRetainedEarningsCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateRetainedEarningsEndpoint))
        .WithSummary("Create RetainedEarnings")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RetainedEarnings.Create);
    }
}
