using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Banks.Create.v1;

/// <summary>
/// Command for creating a new bank entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name>Unique code identifying the bank (e.g., "BNK001", "CHASE-NYC").</param>
/// <param name>The name of the bank or financial institution.</param>
/// <param name>Optional ABA routing number for domestic transfers (9 digits for US banks).</param>
/// <param name>Optional SWIFT/BIC code for international transfers (8 or 11 characters).</param>
/// <param name>Optional physical address of the bank branch.</param>
/// <param name>Optional name of the account officer or primary contact.</param>
/// <param name>Optional bank phone number for inquiries.</param>
/// <param name>Optional bank email address for correspondence.</param>
/// <param name>Optional bank website URL for online banking.</param>
/// <param name>Optional detailed description of the bank or banking relationship.</param>
/// <param name>Optional additional notes or comments about the bank.</param>
/// <param name>Optional image URL for the bank logo.</param>
public sealed record CreateBankCommand(
    string BankCode,
    string Name,
    string? RoutingNumber,
    string? SwiftCode,
    string? Address,
    string? ContactPerson,
    string? PhoneNumber,
    string? Email,
    string? Website,
    string? Description,
    string? Notes,
    string? ImageUrl) : IRequest<BankCreateResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}

