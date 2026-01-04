namespace Accounting.Application.Members.Update.v1;

/// <summary>
/// Command to update a utility member account.
/// </summary>
public sealed record UpdateUtilityMemberCommand(
    DefaultIdType Id,
    string? MemberName,
    string? ServiceAddress,
    string? MailingAddress,
    string? ContactInfo,
    string? AccountStatus,
    DefaultIdType? MeterId,
    string? Email,
    string? PhoneNumber,
    string? EmergencyContact,
    string? ServiceClass,
    string? RateSchedule,
    string? Description,
    string? Notes
) : IRequest<DefaultIdType>;

