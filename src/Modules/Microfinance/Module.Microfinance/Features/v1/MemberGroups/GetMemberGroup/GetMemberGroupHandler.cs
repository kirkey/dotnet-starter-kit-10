using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.MemberGroups.GetMemberGroup;
using FSH.Module.Microfinance.Contracts.v1.MemberGroups;

namespace FSH.Module.Microfinance.Features.v1.MemberGroups.GetMemberGroup;

public class GetMemberGroupHandler(MicrofinanceDbContext context) : IQueryHandler<GetMemberGroupQuery, MemberGroupDto>
{
    public async ValueTask<MemberGroupDto> Handle(GetMemberGroupQuery query, CancellationToken ct)
    {
        var entity = await context.MemberGroups
            .Where(x => x.Id == query.Id)
            .Select(x => new MemberGroupDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("MemberGroup not found");
    }
}
