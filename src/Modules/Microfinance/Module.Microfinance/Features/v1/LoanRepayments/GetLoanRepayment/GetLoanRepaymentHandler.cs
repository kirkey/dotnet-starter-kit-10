using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.LoanRepayments;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.LoanRepayments.GetLoanRepayment;

public record GetLoanRepaymentQuery(Guid Id) : IQuery<LoanRepaymentDto>;

public class GetLoanRepaymentHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanRepaymentQuery, LoanRepaymentDto>
{
    public async ValueTask<LoanRepaymentDto> Handle(GetLoanRepaymentQuery query, CancellationToken ct)
    {
        var entity = await context.LoanRepayments
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanRepaymentDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanRepayment not found");
    }
}
