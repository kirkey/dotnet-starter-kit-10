using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.MemberGroups;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.MemberGroups.GetMemberGroup;

public record GetMemberGroupQuery(Guid Id) : IQuery<MemberGroupDto>;

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
