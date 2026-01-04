using FSH.Framework.Core.Storage.File.Features;

namespace Accounting.Application.Payees.Update.v1;

/// <summary>
/// Command for updating an existing payee entity in the accounting system.
/// Follows the CQRS pattern for command operations with comprehensive validation.
/// </summary>
/// <param name>The unique identifier of the payee to update.</param>
/// <param name>Updated unique code identifying the payee.</param>
/// <param name>Updated name of the payee or vendor company.</param>
/// <param name>Updated mailing or physical address.</param>
/// <param name>Updated default expense account code.</param>
/// <param name>Updated tax identification number.</param>
/// <param name>Updated detailed description of the payee.</param>
/// <param name>Updated additional notes or comments.</param>
/// <param name>Updated image URL for the payee logo or profile picture.</param>
public sealed record UpdatePayeeCommand(
    DefaultIdType Id,
    string PayeeCode,
    string Name,
    string? Address,
    string? ExpenseAccountCode,
    string? Tin,
    string? Description,
    string? Notes,
    string? ImageUrl) : IRequest<PayeeUpdateResponse>
{
    /// <summary>
    /// Optional image payload uploaded by the client. When provided, the image will be saved to storage and ImageUrl will be set to the saved file URL.
    /// </summary>
    public FileUploadCommand? Image { get; init; }
}
