using Microsoft.EntityFrameworkCore;

namespace FSH.Module.Microfinance.Exceptions;

/// <summary>
/// Extension methods for common exception throwing patterns in Microfinance module.
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
/// var loan = await context.Loans.GetByIdOrThrowAsync(id, ct);
/// var member = await context.Members.GetByIdOrThrowAsync(id, ct);
/// await context.Loans.EnsureExistsByIdAsync(loanId, ct);
/// </summary>
public static class MicrofinanceExceptionExtensions
{
    /// <summary>
    /// Gets a Loan by ID or throws LoanNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var loan = await context.Loans
    ///     .Where(l => l.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found Loan or throws exception.</returns>
    /// <exception cref="LoanNotFoundException">Thrown when Loan is not found.</exception>
    public static async Task<Domain.Loan> GetByIdOrThrowAsync(
        this IQueryable<Domain.Loan> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new LoanNotFoundException(id);
    }

    /// <summary>
    /// Gets a Member by ID or throws MemberNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var member = await context.Members
    ///     .Where(m => m.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found Member or throws exception.</returns>
    /// <exception cref="MemberNotFoundException">Thrown when Member is not found.</exception>
    public static async Task<Domain.Member> GetByIdOrThrowAsync(
        this IQueryable<Domain.Member> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new MemberNotFoundException(id);
    }

    /// <summary>
    /// Gets a SavingsAccount by ID or throws SavingsAccountNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var account = await context.SavingsAccounts
    ///     .Where(a => a.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found SavingsAccount or throws exception.</returns>
    /// <exception cref="SavingsAccountNotFoundException">Thrown when SavingsAccount is not found.</exception>
    public static async Task<Domain.SavingsAccount> GetByIdOrThrowAsync(
        this IQueryable<Domain.SavingsAccount> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new SavingsAccountNotFoundException(id);
    }

    /// <summary>
    /// Gets a LoanApplication by ID or throws LoanApplicationNotFoundException if not found.
    /// 
    /// **Usage:**
    /// var application = await context.LoanApplications
    ///     .Where(a => a.Id == command.Id)
    ///     .GetByIdOrThrowAsync(command.Id, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to search for.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found LoanApplication or throws exception.</returns>
    /// <exception cref="LoanApplicationNotFoundException">Thrown when LoanApplication is not found.</exception>
    public static async Task<Domain.LoanApplication> GetByIdOrThrowAsync(
        this IQueryable<Domain.LoanApplication> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        return await query.FirstOrDefaultAsync(cancellationToken)
            ?? throw new LoanApplicationNotFoundException(id);
    }

    /// <summary>
    /// Checks if a Loan exists by ID or throws LoanNotFoundException if not found.
    /// 
    /// Useful when you need to validate existence without loading the full entity.
    /// More efficient than GetByIdOrThrowAsync when entity data is not needed.
    /// 
    /// **Usage:**
    /// await context.Loans.EnsureExistsByIdAsync(command.LoanId, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="LoanNotFoundException">Thrown when Loan is not found.</exception>
    public static async Task EnsureExistsByIdAsync(
        this IQueryable<Domain.Loan> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await query.AnyAsync(cancellationToken);
        if (!exists)
        {
            throw new LoanNotFoundException(id);
        }
    }

    /// <summary>
    /// Checks if a Member exists by ID or throws MemberNotFoundException if not found.
    /// 
    /// Useful when you need to validate existence without loading the full entity.
    /// More efficient than GetByIdOrThrowAsync when entity data is not needed.
    /// 
    /// **Usage:**
    /// await context.Members.EnsureExistsByIdAsync(command.MemberId, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="MemberNotFoundException">Thrown when Member is not found.</exception>
    public static async Task EnsureExistsByIdAsync(
        this IQueryable<Domain.Member> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await query.AnyAsync(cancellationToken);
        if (!exists)
        {
            throw new MemberNotFoundException(id);
        }
    }

    /// <summary>
    /// Checks if a SavingsAccount exists by ID or throws SavingsAccountNotFoundException if not found.
    /// 
    /// Useful when you need to validate existence without loading the full entity.
    /// More efficient than GetByIdOrThrowAsync when entity data is not needed.
    /// 
    /// **Usage:**
    /// await context.SavingsAccounts.EnsureExistsByIdAsync(command.AccountId, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="SavingsAccountNotFoundException">Thrown when SavingsAccount is not found.</exception>
    public static async Task EnsureExistsByIdAsync(
        this IQueryable<Domain.SavingsAccount> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await query.AnyAsync(cancellationToken);
        if (!exists)
        {
            throw new SavingsAccountNotFoundException(id);
        }
    }

    /// <summary>
    /// Checks if a LoanApplication exists by ID or throws LoanApplicationNotFoundException if not found.
    /// 
    /// Useful when you need to validate existence without loading the full entity.
    /// More efficient than GetByIdOrThrowAsync when entity data is not needed.
    /// 
    /// **Usage:**
    /// await context.LoanApplications.EnsureExistsByIdAsync(command.ApplicationId, cancellationToken);
    /// </summary>
    /// <param name="query">The queryable source.</param>
    /// <param name="id">The ID to check.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <exception cref="LoanApplicationNotFoundException">Thrown when LoanApplication is not found.</exception>
    public static async Task EnsureExistsByIdAsync(
        this IQueryable<Domain.LoanApplication> query,
        Guid id,
        CancellationToken cancellationToken)
    {
        var exists = await query.AnyAsync(cancellationToken);
        if (!exists)
        {
            throw new LoanApplicationNotFoundException(id);
        }
    }
}
