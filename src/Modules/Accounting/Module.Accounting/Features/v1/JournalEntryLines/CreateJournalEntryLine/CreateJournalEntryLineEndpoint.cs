using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using FSH.Module.Accounting.Contracts.v1.JournalEntryLines.CreateJournalEntryLine;

namespace FSH.Module.Accounting.Features.v1.JournalEntryLines.CreateJournalEntryLine;

public static class CreateJournalEntryLineEndpoint
{
    public static RouteHandlerBuilder MapCreateJournalEntryLineEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateJournalEntryLineCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateJournalEntryLineEndpoint))
        .WithSummary("Create JournalEntryLine")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.JournalEntryLines.Create);
    }
}
