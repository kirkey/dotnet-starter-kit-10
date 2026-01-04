using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Exceptions;

/// <summary>
/// Exception thrown when a ChartOfAccount (Account) entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a Chart of Account that doesn't exist
/// in the database or has been archived.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Account ID
/// - Operating on archived accounts (depending on context)
/// - Account referenced doesn't belong to current tenant
/// 
/// **Usage:**
/// throw new AccountNotFoundException(accountId);
/// </summary>
public sealed class AccountNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of AccountNotFoundException.
    /// </summary>
    /// <param name="accountId">The ID of the account that was not found.</param>
    public AccountNotFoundException(Guid accountId)
        : base($"Account with id '{accountId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of AccountNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public AccountNotFoundException(string message)
        : base(message) { }
}
