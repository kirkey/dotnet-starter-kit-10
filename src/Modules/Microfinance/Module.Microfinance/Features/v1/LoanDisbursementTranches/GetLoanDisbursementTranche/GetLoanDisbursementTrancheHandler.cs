using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;

namespace FSH.Module.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;

public class GetLoanDisbursementTrancheHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanDisbursementTrancheQuery, LoanDisbursementTrancheDto>
{
    public async ValueTask<LoanDisbursementTrancheDto> Handle(GetLoanDisbursementTrancheQuery query, CancellationToken ct)
    {
        var entity = await context.LoanDisbursementTranches
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanDisbursementTrancheDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanDisbursementTranche not found");
    }
}
