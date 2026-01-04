using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Exceptions;

/// <summary>
/// Exception thrown when an Invoice entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete an Invoice that doesn't exist
/// in the database.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Invoice ID
/// - Attempting to process/approve an invoice that doesn't exist
/// - Invoice referenced doesn't belong to current tenant
/// 
/// **Usage:**
/// throw new InvoiceNotFoundException(invoiceId);
/// </summary>
public sealed class InvoiceNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of InvoiceNotFoundException.
    /// </summary>
    /// <param name="invoiceId">The ID of the invoice that was not found.</param>
    public InvoiceNotFoundException(Guid invoiceId)
        : base($"Invoice with id '{invoiceId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of InvoiceNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public InvoiceNotFoundException(string message)
        : base(message) { }
}
