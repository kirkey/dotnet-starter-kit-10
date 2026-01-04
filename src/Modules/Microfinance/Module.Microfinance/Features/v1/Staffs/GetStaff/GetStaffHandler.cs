using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.Staffs;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.Staffs.GetStaff;

public record GetStaffQuery(Guid Id) : IQuery<StaffDto>;

public class GetStaffHandler(MicrofinanceDbContext context) : IQueryHandler<GetStaffQuery, StaffDto>
{
    public async ValueTask<StaffDto> Handle(GetStaffQuery query, CancellationToken ct)
    {
        var entity = await context.Staffs
            .Where(x => x.Id == query.Id)
            .Select(x => new StaffDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Staff not found");
    }
}
