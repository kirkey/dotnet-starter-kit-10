using FSH.Modules.Microfinance.Data;
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.Members;

namespace FSH.Modules.Microfinance.Features.v1.Members.GetMember;
public record GetMemberQuery(Guid MemberId) : IQuery<MemberDto>;
public class GetMemberHandler(MicrofinanceDbContext context) : IQueryHandler<GetMemberQuery, MemberDto>
{
    public async ValueTask<MemberDto> Handle(GetMemberQuery query, CancellationToken ct)
    {
        var member = await context.Members
            .Where(m => m.Id == query.MemberId)
            .Select(m => new MemberDto(
                m.Id,
                m.MemberNumber,
                m.FirstName,
                m.LastName,
                m.MiddleName,
                m.FullName,
                m.Email,
                m.PhoneNumber,
                m.DateOfBirth,
                m.Gender,
                m.Address,
                m.NationalId,
                m.Occupation,
                m.MonthlyIncome,
                m.JoinDate,
                m.IsActive,
                m.CreatedOnUtc,
                m.CreatedByUserName))
            .FirstOrDefaultAsync(ct);
        return member ?? throw new NotFoundException("Member not found");
    }
}
