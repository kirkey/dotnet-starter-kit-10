using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using FSH.Module.Accounting.Contracts.v1.RecurringJournalEntries.CreateRecurringJournalEntry;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Module.Accounting.Features.v1.RecurringJournalEntries.CreateRecurringJournalEntry;

public static class CreateRecurringJournalEntryEndpoint
{
    public static RouteHandlerBuilder MapCreateRecurringJournalEntryEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateRecurringJournalEntryCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting//{id}", id);
        })
        .WithName(nameof(CreateRecurringJournalEntryEndpoint))
        .WithSummary("Create RecurringJournalEntry")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.RecurringJournalEntries.Create);
    }
}
