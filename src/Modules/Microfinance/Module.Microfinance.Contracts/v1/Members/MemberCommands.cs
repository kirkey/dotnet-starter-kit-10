namespace FSH.Module.Microfinance.Contracts.v1.Members;

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
    decimal? MonthlyIncome);

public record UpdateMemberCommand(
    string? FirstName,
    string? LastName,
    string? MiddleName,
    string? Email,
    string? PhoneNumber,
    DateTimeOffset? DateOfBirth,
    string? Gender,
    string? Address,
    string? NationalId,
    string? Occupation,
    decimal? MonthlyIncome);

public record ActivateMemberCommand(Guid MemberId);
public record DeactivateMemberCommand(Guid MemberId);
public record DeleteMemberCommand(Guid MemberId);
