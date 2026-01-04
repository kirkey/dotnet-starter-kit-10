using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.Members;
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.Members.GetMember;

public record GetMemberQuery(Guid Id) : IQuery<MemberDto>;

public class GetMemberHandler(AccountingDbContext context) : IQueryHandler<GetMemberQuery, MemberDto>
{
    public async ValueTask<MemberDto> Handle(GetMemberQuery query, CancellationToken ct)
    {
        var entity = await context.Members
            .Where(x => x.Id == query.Id)
            .Select(x => new MemberDto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("Member not found");
    }
}
