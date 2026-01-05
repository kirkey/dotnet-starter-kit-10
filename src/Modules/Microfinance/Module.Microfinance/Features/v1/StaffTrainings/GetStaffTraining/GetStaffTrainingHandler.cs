using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.StaffTrainings;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.StaffTrainings.GetStaffTraining;

namespace FSH.Module.Microfinance.Features.v1.StaffTrainings.GetStaffTraining;

public class GetStaffTrainingHandler(MicrofinanceDbContext context) : IQueryHandler<GetStaffTrainingQuery, StaffTrainingDto>
{
    public async ValueTask<StaffTrainingDto> Handle(GetStaffTrainingQuery query, CancellationToken ct)
    {
        var entity = await context.StaffTrainings
            .Where(x => x.Id == query.Id)
            .Select(x => new StaffTrainingDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("StaffTraining not found");
    }
}
