using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.DeleteCheck;

/// <summary>
/// Command to delete an existing check from the system.
/// </summary>
/// <param name="Id">Check ID to delete (must exist, preferably Draft status)</param>
public record DeleteCheckCommand(Guid Id) : ICommand;

/// <summary>
/// Handler for deleting a check from the system.
/// </summary>
/// <remarks>
/// Responsibility: Delete check entity with optional state validation.
/// 
/// Execution Flow:
/// 1. Find check by Id using FindAsync(); throw NotFoundException if not found
/// 2. Optional: Validate check status is Draft (printed/cleared checks should not be deleted)
/// 3. Remove check from Checks DbSet
/// 4. Persist deletion via SaveChangesAsync
/// 5. Return Unit (void result)
/// 
/// Business Rules (Implementation Note):
/// - Draft checks: Can be deleted freely (not yet processed)
/// - Printed checks: Should not be deleted (audit trail needed)
/// - Issued checks: Must be voided instead (GL reconciliation required)
/// - Cleared checks: Cannot be deleted (reconciliation history)
/// 
/// Current Implementation: No status validation; consider adding if delete audit is critical.
/// 
/// Permissions: Requires authenticated user with check delete permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if check with specified ID not found
/// </remarks>
public class DeleteCheckHandler(AccountingDbContext context) : ICommandHandler<DeleteCheckCommand>
{
    public async ValueTask<Unit> Handle(DeleteCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Check not found");

        context.Checks.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}