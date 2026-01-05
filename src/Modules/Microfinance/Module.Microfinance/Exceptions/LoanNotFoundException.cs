using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Microfinance.Exceptions;

/// <summary>
/// Exception thrown when a Loan entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a Loan that doesn't exist
/// in the database or has been deleted.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Loan ID
/// - Loan referenced doesn't belong to current tenant
/// - Operating on inactive/archived loans (depending on context)
/// 
/// **Usage:**
/// throw new LoanNotFoundException(loanId);
/// </summary>
public sealed class LoanNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of LoanNotFoundException.
    /// </summary>
    /// <param name="loanId">The ID of the loan that was not found.</param>
    public LoanNotFoundException(Guid loanId)
        : base($"Loan with id '{loanId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of LoanNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public LoanNotFoundException(string message)
        : base(message) { }
}
