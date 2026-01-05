using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Microfinance.Exceptions;

/// <summary>
/// Exception thrown when a SavingsAccount entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a SavingsAccount that doesn't exist
/// in the database or has been deleted.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid SavingsAccount ID
/// - Account referenced doesn't belong to current tenant
/// - Operating on inactive/closed accounts (depending on context)
/// 
/// **Usage:**
/// throw new SavingsAccountNotFoundException(accountId);
/// </summary>
public sealed class SavingsAccountNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountNotFoundException.
    /// </summary>
    /// <param name="accountId">The ID of the savings account that was not found.</param>
    public SavingsAccountNotFoundException(Guid accountId)
        : base($"SavingsAccount with id '{accountId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of SavingsAccountNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public SavingsAccountNotFoundException(string message)
        : base(message) { }
}
