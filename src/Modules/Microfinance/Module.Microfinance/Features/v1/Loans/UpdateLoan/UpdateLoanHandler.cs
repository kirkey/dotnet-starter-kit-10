using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Exceptions;

namespace FSH.Module.Microfinance.Features.v1.Loans.UpdateLoan;

/// <summary>
/// Handles the update of an existing loan.
/// 
/// **Purpose:**
/// Updates loan details in the current tenant's context.
/// 
/// **Business Rules:**
/// - Loan must exist in the system
/// - Loan name is required (validated in UpdateLoanValidator if exists)
/// 
/// **Dependencies:**
/// - MicrofinanceDbContext: For database operations
/// </summary>
public sealed class UpdateLoanHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateLoanCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateLoanCommand command, CancellationToken ct)
    {
        var entity = await context.Loans
            .Where(x => x.Id == command.Id)
            .GetByIdOrThrowAsync(command.Id, ct);
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
