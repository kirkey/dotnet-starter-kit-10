using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Data;
using Mediator;

using FSH.Module.Accounting.Contracts.v1.Banks.DeleteBank;namespace FSH.Module.Accounting.Features.v1.Banks.DeleteBank;

/// <summary>
/// Handler for deleting a bank account from the system.
/// </summary>
/// <remarks>
/// Responsibility: Delete bank entity without business rule validation (archival approach).
/// 
/// Execution Flow:
/// 1. Find bank by Id using FindAsync(); throw NotFoundException if not found
/// 2. Remove bank from Banks DbSet
/// 3. Persist deletion via SaveChangesAsync
/// 4. Return Unit (void result)
/// 
/// Design Note: Handler allows bank deletion without checking for related transactions.
/// Consider adding business rule validation if cash flow historical integrity is required.
/// Currently assumes archived/inactive banks are soft-deleted via IsActive flag rather than hard deletion.
/// 
/// Permissions: Requires authenticated user with bank delete permission
/// 
/// Exceptions:
/// - NotFoundException: Thrown if bank with specified ID not found
/// </remarks>
public class DeleteBankHandler(AccountingDbContext context) : ICommandHandler<DeleteBankCommand>
{
    public async ValueTask<Unit> Handle(DeleteBankCommand command, CancellationToken ct)
    {
        var entity = await context.Banks.FindAsync(command.Id, ct)
            ?? throw new NotFoundException("Bank not found");
        
        context.Banks.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
