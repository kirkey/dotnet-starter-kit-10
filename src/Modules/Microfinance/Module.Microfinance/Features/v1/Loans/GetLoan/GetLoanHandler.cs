using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Exceptions;

namespace FSH.Module.Microfinance.Features.v1.Loans.GetLoan;

/// <summary>
/// Handles retrieval of a single loan by ID.
/// 
/// **Purpose:**
/// Retrieves detailed information about a specific loan.
/// 
/// **Business Rules:**
/// - Loan must exist in the system
/// - Loan must belong to current tenant
/// 
/// **Dependencies:**
/// - MicrofinanceDbContext: For database queries
/// </summary>
public sealed class GetLoanHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanQuery, LoanDto>
{
    public async ValueTask<LoanDto> Handle(GetLoanQuery query, CancellationToken ct)
    {
        var entity = await context.Loans
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        if (entity is null)
        {
            throw new LoanNotFoundException(query.Id);
        }
        
        return entity;
    }
}
