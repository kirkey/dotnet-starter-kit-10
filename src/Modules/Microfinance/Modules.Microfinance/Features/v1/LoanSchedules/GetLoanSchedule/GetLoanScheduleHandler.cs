using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.LoanSchedules;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.LoanSchedules.GetLoanSchedule;

public record GetLoanScheduleQuery(Guid Id) : IQuery<LoanScheduleDto>;

public class GetLoanScheduleHandler(MicrofinanceDbContext context) : IQueryHandler<GetLoanScheduleQuery, LoanScheduleDto>
{
    public async ValueTask<LoanScheduleDto> Handle(GetLoanScheduleQuery query, CancellationToken ct)
    {
        var entity = await context.LoanSchedules
            .Where(x => x.Id == query.Id)
            .Select(x => new LoanScheduleDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("LoanSchedule not found");
    }
}
