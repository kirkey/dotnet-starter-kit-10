using FSH.Framework.Core.Context;
using FSH.Module.Microfinance.Contracts.v1.Members;
using FSH.Module.Microfinance.Data;
using FSH.Module.Microfinance.Domain;

namespace FSH.Module.Microfinance.Features.v1.Members.CreateMember;
public record CreateMemberCommand(
    string MemberNumber,
    string FirstName,
    string LastName,
    string? MiddleName,
    string? Email,
    string? PhoneNumber,
    DateTimeOffset? DateOfBirth,
    string? Gender,
    string? Address,
    string? NationalId,
    string? Occupation,
    decimal? MonthlyIncome) : ICommand<Guid>;
public class CreateMemberHandler(
    ICurrentUser currentUser,
    MicrofinanceDbContext context) : ICommandHandler<CreateMemberCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMemberCommand command, CancellationToken ct)
    {
        var member = Member.Create(
            command.MemberNumber,
            command.FirstName,
            command.LastName,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.MiddleName,
            command.Email,
            command.PhoneNumber,
            command.DateOfBirth,
            command.Gender,
            command.Address,
            command.NationalId,
            command.Occupation,
            command.MonthlyIncome);
        context.Members.Add(member);
        await context.SaveChangesAsync(ct);
        return member.Id;
    }
}
