using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Command to create a new member.
/// </summary>
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
