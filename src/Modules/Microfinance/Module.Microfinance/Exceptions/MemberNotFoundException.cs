using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Microfinance.Exceptions;

/// <summary>
/// Exception thrown when a Member entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, or delete a Member that doesn't exist
/// in the database or has been deleted.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Member ID
/// - Member referenced doesn't belong to current tenant
/// - Operating on inactive/archived members (depending on context)
/// 
/// **Usage:**
/// throw new MemberNotFoundException(memberId);
/// </summary>
public sealed class MemberNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of MemberNotFoundException.
    /// </summary>
    /// <param name="memberId">The ID of the member that was not found.</param>
    public MemberNotFoundException(Guid memberId)
        : base($"Member with id '{memberId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of MemberNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public MemberNotFoundException(string message)
        : base(message) { }
}
