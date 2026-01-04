using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Checks;
using FSH.Module.Accounting.Contracts.v1.Checks.GetCheck;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.Checks.GetCheck;

/// <summary>
/// Handler for retrieving a single check by ID with complete DTO projection.
/// </summary>
/// <remarks>
/// Responsibility: Execute the check query and return a DTO with 18 returned fields.
/// 
/// Execution Flow:
/// 1. Query Checks DbSet by Id using Where(x => x.Id == query.Id)
/// 2. Project to CheckDto with 18 fields: Id, CheckNumber, CheckDate, CheckType, PayeeId, PayeeName, 
///    BankAccountId, AccountNumber, Amount, Status, PrintedDate, ClearedDate, ClearedBy, JournalEntryId, 
///    ReferenceNumber, Notes, IsActive, CreatedOnUtc
/// 3. Execute FirstOrDefaultAsync() to retrieve single result
/// 4. Throw NotFoundException if entity not found
/// 
/// Returned Fields (CheckDto): Id, CheckNumber, CheckDate, CheckType, PayeeId, PayeeName, 
/// BankAccountId, AccountNumber, Amount, Status, PrintedDate, ClearedDate, ClearedBy, 
/// JournalEntryId, ReferenceNumber, Notes, IsActive, CreatedOnUtc
/// 
/// Status Tracking: Check tracks lifecycle (Draft, Printed, Issued, Cleared, Voided)
/// Reconciliation: ClearedDate and ClearedBy track bank reconciliation processing
/// GL Link: JournalEntryId links check to corresponding GL posting
/// 
/// Permissions: Requires authenticated user (any authorized role)
/// 
/// Exceptions:
/// - NotFoundException: Thrown if check with specified ID not found
/// </remarks>
public class GetCheckHandler(AccountingDbContext context) : IQueryHandler<GetCheckQuery, CheckDto>
{
    public async ValueTask<CheckDto> Handle(GetCheckQuery query, CancellationToken ct)
    {
        var entity = await context.Checks
            .Where(x => x.Id == query.Id)
            .Select(x => new CheckDto(
                x.Id,
                x.CheckNumber,
                x.CheckDate,
                x.CheckType,
                x.PayeeId,
                x.PayeeName,
                x.BankAccountId,
                x.AccountNumber,
                x.Amount,
                x.Status,
                x.PrintedDate,
                x.ClearedDate,
                x.ClearedBy,
                x.JournalEntryId,
                x.ReferenceNumber,
                x.Notes,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Check not found");
    }
}
