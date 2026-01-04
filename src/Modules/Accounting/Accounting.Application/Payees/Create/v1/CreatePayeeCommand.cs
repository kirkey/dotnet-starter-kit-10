using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Payees.Create.v1;

/// <summary>
/// Command for creating a new payee entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name>Unique code identifying the payee (e.g., "VEND001", "UTIL-ELEC").</param>
/// <param name>The name of the payee or vendor company.</param>
/// <param name>Optional mailing or physical address for correspondence and check printing.</param>
/// <param name>Optional default expense account code for automated journal entries.</param>
/// <param name>Optional tax identification number for 1099 reporting and compliance.</param>
/// <param name>Optional detailed description of the payee's business or services.</param>
/// <param name>Optional additional notes or comments about the payee.</param>
/// <param name>Optional image URL for the payee logo or profile picture.</param>
public sealed record CreatePayeeCommand(
    string PayeeCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? Tin,
    string? Description,
    string? Notes,
    string? ImageUrl) : IRequest<PayeeCreateResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image is uploaded to storage and ImageUrl is set from the saved file name.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
