using FSH.Framework.Core.Context;
using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.Loans.CreateLoan;

/// <summary>
/// Handles the creation of a new loan.
/// 
/// **Purpose:**
/// Creates a new loan in the current tenant's context using the provided details.
/// 
/// **Business Rules:**
/// - Loan name is required (validated in CreateLoanValidator)
/// - Name must be 256 characters or less
/// 
/// **Dependencies:**
/// - MicrofinanceDbContext: For database persistence
/// - ICurrentUser: For tenant isolation and audit tracking
/// </summary>
public sealed class CreateLoanHandler(ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateLoanCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateLoanCommand command, CancellationToken ct)
    {
        var entity = Loan.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.Loans.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
