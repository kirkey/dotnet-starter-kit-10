using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs.GetLoanWriteOff;
using FSH.Module.Microfinance.Contracts.v1.LoanWriteOffs;

namespace FSH.Module.Microfinance.Features.v1.LoanWriteOffs.GetLoanWriteOff;

public class GetLoanWriteOffHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanWriteOffQuery, LoanWriteOffDto>
{
    public async ValueTask<LoanWriteOffDto> Handle(GetLoanWriteOffQuery query, CancellationToken ct)
    {
        var entity = await context.LoanWriteOffs
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanWriteOffDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanWriteOff not found");
    }
}
