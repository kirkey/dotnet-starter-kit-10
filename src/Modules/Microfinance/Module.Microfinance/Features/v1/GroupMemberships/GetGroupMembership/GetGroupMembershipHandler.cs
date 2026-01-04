using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.GroupMemberships;
using FSH.Module.Microfinance.Data;

namespace FSH.Module.Microfinance.Features.v1.GroupMemberships.GetGroupMembership;

public record GetGroupMembershipQuery(Guid Id) : IQuery<GroupMembershipDto>;

public class GetGroupMembershipHandler(MicrofinanceDbContext context) : IQueryHandler<GetGroupMembershipQuery, GroupMembershipDto>
{
    public async ValueTask<GroupMembershipDto> Handle(GetGroupMembershipQuery query, CancellationToken ct)
    {
        var entity = await context.GroupMemberships
            .Where(x => x.Id == query.Id)
            .Select(x => new GroupMembershipDto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("GroupMembership not found");
    }
}
