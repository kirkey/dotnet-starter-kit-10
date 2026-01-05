using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Contracts.v1.Members;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Members;namespace FSH.Module.Microfinance.Features.v1.Members.GetMember;
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
