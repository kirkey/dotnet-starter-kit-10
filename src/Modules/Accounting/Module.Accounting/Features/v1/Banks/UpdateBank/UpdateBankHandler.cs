using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.Banks.UpdateBank;namespace FSH.Module.Accounting.Features.v1.Banks.UpdateBank;

/// <summary>
/// Handler for updating bank account metadata and banking details.
/// </summary>
/// <remarks>
/// Responsibility: Update bank mutable fields and persist changes.
/// 
/// Execution Flow:
/// 1. Find bank by Id using FindAsync(); throw NotFoundException if not found
/// 2. Call entity.Update() with all 11 parameters to apply changes
/// 3. Persist changes to database via SaveChangesAsync
/// 4. Return updated bank ID
/// 
/// Updateable Fields: BankName, BankCode, Address, ContactName, ContactPhone, RoutingNumber, SwiftCode, 
/// CurrencyCode, OpeningBalance, IsDefault, Description
/// Immutable Fields: Id, TenantId, CurrentBalance (changed only by transactions), CreatedBy, CreatedOnUtc
/// 
/// Notes: 
/// - CurrentBalance is managed by transaction posting (invoices, payments, journal entries)
/// - Opening balance can be adjusted but subsequent changes should use GL adjustment entries
/// - IsDefault can be used for automatic account selection in transaction processing
/// 
/// Permissions: Requires authenticated user with bank update permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if bank with specified ID not found
/// - BadRequestException: Thrown by validation if BankName is empty
/// </remarks>
public class UpdateBankHandler(AccountingDbContext context) : ICommandHandler<UpdateBankCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateBankCommand command, CancellationToken ct)
    {
        var entity = await context.Banks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bank not found");
        
        entity.Update(
            command.BankName,
            command.BankCode,
            command.Address,
            command.ContactName,
            command.ContactPhone,
            command.RoutingNumber,
            command.SwiftCode,
            command.CurrencyCode,
            command.OpeningBalance,
            command.IsDefault,
            command.Description);
        
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
