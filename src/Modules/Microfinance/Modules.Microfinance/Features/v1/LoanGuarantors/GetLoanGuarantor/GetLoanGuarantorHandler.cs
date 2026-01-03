using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LoanGuarantors;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanGuarantors.GetLoanGuarantor;

public record GetLoanGuarantorQuery(Guid Id) : IQuery<LoanGuarantorDto>;

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
