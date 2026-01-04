namespace Accounting.Application.Members.Create.v1;

/// <summary>
/// Command to create a new member account.
/// </summary>
public sealed record CreateUtilityMemberCommand(
    string MemberNumber,
    string MemberName,
    string ServiceAddress,
    DateTime MembershipDate,
    string? MailingAddress,
    string? ContactInfo,
    string AccountStatus,
    DefaultIdType? MeterId,
    string? Email,
    string? PhoneNumber,
    string? EmergencyContact,
    string? ServiceClass,
    DefaultIdType? RateScheduleId,
    string? RateSchedule,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

