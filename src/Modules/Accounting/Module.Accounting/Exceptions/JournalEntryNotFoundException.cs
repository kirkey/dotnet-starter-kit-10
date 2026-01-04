using FSH.Framework.Core.Exceptions;

namespace FSH.Module.Accounting.Exceptions;

/// <summary>
/// Exception thrown when a JournalEntry entity is not found.
/// 
/// **Purpose:**
/// Raised when attempting to retrieve, update, post, or delete a Journal Entry that doesn't exist
/// in the database.
/// 
/// **HTTP Mapping:**
/// Returns 404 Not Found status code
/// 
/// **When Used:**
/// - Get/Update/Delete operations with invalid Journal Entry ID
/// - Attempting to post/approve a journal entry that doesn't exist
/// - Journal entry referenced doesn't belong to current tenant
/// 
/// **Usage:**
/// throw new JournalEntryNotFoundException(journalEntryId);
/// </summary>
public sealed class JournalEntryNotFoundException : NotFoundException
{
    /// <summary>
    /// Initializes a new instance of JournalEntryNotFoundException.
    /// </summary>
    /// <param name="journalEntryId">The ID of the journal entry that was not found.</param>
    public JournalEntryNotFoundException(Guid journalEntryId)
        : base($"Journal Entry with id '{journalEntryId}' not found.") { }

    /// <summary>
    /// Initializes a new instance of JournalEntryNotFoundException with custom message.
    /// </summary>
    /// <param name="message">Custom error message.</param>
    public JournalEntryNotFoundException(string message)
        : base(message) { }
}
