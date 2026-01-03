using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LoanDisbursementTranches;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanDisbursementTranches.GetLoanDisbursementTranche;

public record GetLoanDisbursementTrancheQuery(Guid Id) : IQuery<LoanDisbursementTrancheDto>;

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
