using FSH.Modules.Microfinance.Contracts.v1.StaffTrainings;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.StaffTrainings.GetStaffTrainings;

public record GetStaffTrainingsQuery(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<StaffTrainingsPagedResponse>;

public class GetStaffTrainingsHandler(MicrofinanceDbContext context) : IQueryHandler<GetStaffTrainingsQuery, StaffTrainingsPagedResponse>
{
    public async ValueTask<StaffTrainingsPagedResponse> Handle(GetStaffTrainingsQuery query, CancellationToken ct)
    {
        var queryable = context.StaffTrainings.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new StaffTrainingSummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new StaffTrainingsPagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
