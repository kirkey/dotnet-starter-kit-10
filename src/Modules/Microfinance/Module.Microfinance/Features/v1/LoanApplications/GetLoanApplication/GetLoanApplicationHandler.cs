using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.LoanApplications;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.LoanApplications;namespace FSH.Module.Microfinance.Features.v1.LoanApplications.GetLoanApplication;

public class GetLoanApplicationHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanApplicationQuery, LoanApplicationDto>
{
    public async ValueTask<LoanApplicationDto> Handle(GetLoanApplicationQuery query, CancellationToken ct)
    {
        var entity = await context.LoanApplications
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanApplicationDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanApplication not found");
    }
}
