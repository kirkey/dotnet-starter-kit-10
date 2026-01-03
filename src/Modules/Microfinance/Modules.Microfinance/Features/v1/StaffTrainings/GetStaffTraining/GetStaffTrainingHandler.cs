using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.StaffTrainings;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.GetStaffTraining;

public record GetStaffTrainingQuery(Guid Id) : IQuery<StaffTrainingDto>;

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
