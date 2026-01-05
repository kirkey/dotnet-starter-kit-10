using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Exceptions;

namespace FSH.Module.Microfinance.Features.v1.Loans.DeleteLoan;

/// <summary>
/// Handles the deletion of a loan.
/// 
/// **Purpose:**
/// Removes a loan from the system.
/// 
/// **Business Rules:**
/// - Loan must exist in the system
/// - Loan must belong to current tenant
/// 
/// **Dependencies:**
/// - MicrofinanceDbContext: For database operations
/// </summary>
public sealed class DeleteLoanHandler(MicrofinanceDbContext context) : ICommandHandler<DeleteLoanCommand, Guid>
{
    public async ValueTask<Guid> Handle(DeleteLoanCommand command, CancellationToken ct)
    {
        var entity = await context.Loans
            .Where(x => x.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, ct);
        
        context.Loans.Remove(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
