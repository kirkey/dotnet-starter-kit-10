using Mediator;

namespace FSH.Module.Microfinance.Contracts.v1.Members;

/// <summary>
/// Command to update an existing member.
/// </summary>
public record UpdateMemberCommand(
    Guid Id,
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
    decimal? MonthlyIncome) : ICommand<Guid>;
