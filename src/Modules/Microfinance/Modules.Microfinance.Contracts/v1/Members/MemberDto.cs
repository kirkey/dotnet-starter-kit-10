namespace FSH.Modules.Microfinance.Contracts.v1.Members;

public record MemberDto(
    Guid Id,
    string MemberNumber,
    string FirstName,
    string LastName,
    string? MiddleName,
    string FullName,
    string? Email,
    string? PhoneNumber,
    DateTimeOffset? DateOfBirth,
    string? Gender,
    string? Address,
    string? NationalId,
    string? Occupation,
    decimal? MonthlyIncome,
    DateTimeOffset JoinDate,
    bool IsActive,
    DateTimeOffset CreatedOnUtc,
    string? CreatedByUserName);

public record MemberSummaryDto(
    Guid Id,
    string MemberNumber,
    string FullName,
    string? Email,
    string? PhoneNumber,
    bool IsActive,
    DateTimeOffset JoinDate);
