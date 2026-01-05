using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors.GetLoanGuarantor;
using FSH.Module.Microfinance.Contracts.v1.LoanGuarantors;

namespace FSH.Module.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantor;

public class GetLoanGuarantorHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanGuarantorQuery, LoanGuarantorDto>
{
    public async ValueTask<LoanGuarantorDto> Handle(GetLoanGuarantorQuery query, CancellationToken ct)
    {
        var entity = await context.LoanGuarantors
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanGuarantorDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanGuarantor not found");
    }
}
