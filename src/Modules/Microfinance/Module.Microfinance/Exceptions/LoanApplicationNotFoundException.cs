using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Microfinance.Exceptions;

/// <summary>
/// Exception thrown when a LoanApplication entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a LoanApplication that doesn't exist
/// in the database or has been deleted.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid LoanApplication ID
/// - Application referenced doesn't belong to current tenant
/// - Operating on inactive/archived applications (depending on context)
/// 
/// **Usage:**
/// throw new LoanApplicationNotFoundException(applicationId);
/// </summary>
public sealed class LoanApplicationNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationNotFoundException.
    /// </summary>
    /// <param name="applicationId">The ID of the loan application that was not found.</param>
    public LoanApplicationNotFoundException(Guid applicationId)
        : base($"LoanApplication with id '{applicationId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of LoanApplicationNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public LoanApplicationNotFoundException(string message)
        : base(message) { }
}
