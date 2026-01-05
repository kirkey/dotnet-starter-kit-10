using FSH.Framework.Core.Exceptions;
using FSH.Module.Microfinance.Data;

using FSH.Module.Microfinance.Contracts.v1.Members;namespace FSH.Module.Microfinance.Features.v1.Members.UpdateMember;
public class UpdateMemberHandler(MicrofinanceDbContext context) : ICommandHandler<UpdateMemberCommand, Guid>
{
    public async ValueTask<Guid> Handle(UpdateMemberCommand command, CancellationToken ct)
    {
        var member = await context.Members.FindAsync([command.MemberId], ct)
            ?? throw new NotFoundException("Member not found");
        member.Update(
            command.FirstName,
            command.LastName,
            command.MiddleName,
            command.Email,
            command.PhoneNumber,
            command.DateOfBirth,
            command.Gender,
            command.Address,
            command.NationalId,
            command.Occupation,
            command.MonthlyIncome);
        await context.SaveChangesAsync(ct);
        return member.Id;
    }
}
