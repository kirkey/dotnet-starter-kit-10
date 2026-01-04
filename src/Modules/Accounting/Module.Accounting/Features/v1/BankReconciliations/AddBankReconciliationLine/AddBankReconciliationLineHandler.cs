using FSH.Framework.Shared.Identity;
using FSH.Module.Accounting.Data;
using FSH.Module.Accounting.Domain;
using FSH.Module.Accounting.Contracts.v1.BankReconciliations;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Features.v1.BankReconciliations.AddBankReconciliationLine;

/// <summary>
/// Handler for adding a line to an existing BankReconciliation aggregate.
/// Validates the parent reconciliation and records the created line with audit/tenant metadata.
/// </summary>
public class AddBankReconciliationLineHandler(AccountingDbContext context, ICurrentUser currentUser) : ICommandHandler<AddBankReconciliationLineCommand, Guid>
{
    public async ValueTask<Guid> Handle(AddBankReconciliationLineCommand command, CancellationToken ct)
    {
        var recon = await context.BankReconciliations.FirstOrDefaultAsync(x => x.Id == command.BankReconciliationId, ct)
            ?? throw new BadRequestException("BankReconciliation not found");

        var entity = BankReconciliationLine.Create(
            command.BankReconciliationId,
            command.TransactionId,
            command.TransactionDate,
            command.Amount,
            command.Description,
            recon.TenantId,
            currentUser.GetUserId(),
            currentUser.Name ?? "System");

        context.BankReconciliationLines.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}