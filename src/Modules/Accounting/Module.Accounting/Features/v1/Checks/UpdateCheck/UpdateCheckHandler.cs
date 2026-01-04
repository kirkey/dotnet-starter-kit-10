using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Checks.UpdateCheck;
using FSH.Module.Accounting.Data;
using Mediator;

namespace FSH.Module.Accounting.Features.v1.Checks.UpdateCheck;

/// <summary>
/// Handler for updating check payment details and metadata.
/// </summary>
/// <remarks>
/// Responsibility: Update check mutable fields and persist changes.
/// 
/// Execution Flow:
/// 1. Find check by Id using FindAsync(); throw NotFoundException if not found
/// 2. Call entity.Update() with 10 parameters to apply changes
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return updated check ID
/// 
/// Updateable Fields: CheckNumber, CheckDate, CheckType, BankAccountId, AccountNumber, Amount, 
/// PayeeName, PayeeId, ReferenceNumber, Notes
/// Immutable Fields: Id, TenantId, Status (changed via special operations), PrintedDate, ClearedDate, 
/// JournalEntryId, CreatedBy, CreatedOnUtc
/// 
/// Design Note: Check number, amount, and bank account can be updated if check is still in Draft status.
/// Once printed or posted, restrictions may apply via domain method validation.
/// 
/// Permissions: Requires authenticated user with check update permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if check with specified ID not found
/// - BadRequestException: Thrown by validation if CheckNumber or Amount invalid
/// </remarks>
public class UpdateCheckHandler(AccountingDbContext context) : ICommandHandler<UpdateCheckCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateCheckCommand command, CancellationToken ct)
    {
        var entity = await context.Checks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Check not found");

        entity.Update(
            command.CheckNumber,
            command.CheckDate,
            command.CheckType,
            command.BankAccountId,
            command.AccountNumber,
            command.Amount,
            command.PayeeName,
            command.PayeeId,
            command.ReferenceNumber,
            command.Notes);

        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}