using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.Loans;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Loans.GetLoan;

public record GetLoanQuery(Guid Id) : IQuery<LoanDto>;

public class GetLoanHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanQuery, LoanDto>
{
    public async ValueTask<LoanDto> Handle(GetLoanQuery query, CancellationToken ct)
    {
        var entity = await context.Loans
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Loan not found");
    }
}
