using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Accounting.Exceptions;

/// <summary>
/// Extension methods for common exception throwing patterns in Accounting module.
/// 
/// **Purpose:**
/// Provides fluent, reusable methods for validating entity existence and throwing
/// appropriate exceptions. This reduces code duplication and ensures consistent
/// exception handling across all handlers.
/// 
/// **Benefits:**
/// - Single point of exception definition
/// - Reduced code duplication
/// - Consistent error messages
/// - Fluent API for validation
/// - Easier testing and maintenance
/// 
/// **Usage Examples:**
/// var account = await context.ChartOfAccounts.GetByIdOrThrowAsync(id, ct);
/// var journal = await context.JournalEntries.GetByIdOrThrowAsync(id, ct);
/// var invoice = await context.Invoices.GetByIdOrThrowAsync(id, ct);
/// </summary>
public static class AccountingExceptionExtensions
{
    /// <summary>
    /// Gets a ChartOfAccount by ID or throws AccountNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var account = await context.ChartOfAccounts
    ///     .Where(a => a.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found ChartOfAccount or throws exception.</returns>
    /// <exception cref="AccountNotFoundException">Thrown when Account is not found.</exception>
    public static async Task<Domain.ChartOfAccount> GetAccountByIdOrThrowAsync(
        this IQueryable<Domain.ChartOfAccount> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new AccountNotFoundException(id);
    }

    /// <summary>
    /// Gets a JournalEntry by ID or throws JournalEntryNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var journal = await context.JournalEntries
    ///     .Where(j => j.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found JournalEntry or throws exception.</returns>
    /// <exception cref="JournalEntryNotFoundException">Thrown when JournalEntry is not found.</exception>
    public static async Task<Domain.JournalEntry> GetJournalByIdOrThrowAsync(
        this IQueryable<Domain.JournalEntry> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new JournalEntryNotFoundException(id);
    }

    /// <summary>
    /// Gets an Invoice by ID or throws InvoiceNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var invoice = await context.Invoices
    ///     .Where(i => i.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found Invoice or throws exception.</returns>
    /// <exception cref="InvoiceNotFoundException">Thrown when Invoice is not found.</exception>
    public static async Task<Domain.Invoice> GetInvoiceByIdOrThrowAsync(
        this IQueryable<Domain.Invoice> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvoiceNotFoundException(id);
    }
}
