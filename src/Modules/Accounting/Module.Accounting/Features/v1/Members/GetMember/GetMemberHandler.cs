using FSH.Framework.Core.Exceptions;
using FSH.Module.Accounting.Contracts.v1.Members;
using FSH.Module.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

using FSH.Module.Accounting.Contracts.v1.Members.GetMember;

namespace FSH.Module.Accounting.Features.v1.Members.GetMember;

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
