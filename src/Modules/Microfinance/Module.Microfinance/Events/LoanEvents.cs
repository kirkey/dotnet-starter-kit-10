namespace FSH.Module.Microfinance.Events;

/// <summary>
/// Base class for Loan-related domain events.
/// </summary>
public abstract class LoanDomainEvent : MicrofinanceDomainEvent
{
    public override string AggregateType => nameof(Domain.Loan);
}

/// <summary>
/// Event raised when a Loan has been created.
/// 
/// **Trigger:** After successful Loan creation
/// **Handlers:** Can send notifications, update search index, log activity
/// **Usage:** Subscribe to track all new loans created in the system
/// </summary>
public sealed class LoanCreatedEvent : LoanDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanCreatedEvent.
    /// </summary>
    /// <param name="loanId">The ID of the created loan.</param>
    /// <param name="name">The name of the created loan.</param>
    /// <param name="createdBy">The user ID who created the loan.</param>
    public LoanCreatedEvent(Guid loanId, string name, Guid createdBy)
    {
        AggregateId = loanId;
        Name = name;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the name of the created loan.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who created the loan.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a Loan has been updated.
/// 
/// **Trigger:** After successful Loan update
/// **Handlers:** Can update search index, send notifications, log changes
/// **Usage:** Track modifications to loans
/// </summary>
public sealed class LoanUpdatedEvent : LoanDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanUpdatedEvent.
    /// </summary>
    /// <param name="loanId">The ID of the updated loan.</param>
    /// <param name="name">The updated name.</param>
    /// <param name="modifiedBy">The user ID who modified the loan.</param>
    public LoanUpdatedEvent(Guid loanId, string name, Guid modifiedBy)
    {
        AggregateId = loanId;
        Name = name;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who modified the loan.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a Loan has been deleted.
/// 
/// **Trigger:** After successful Loan deletion
/// **Handlers:** Can remove from search index, send notifications, log deletion
/// **Usage:** Track loan deletions for audit and compliance
/// </summary>
public sealed class LoanDeletedEvent : LoanDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanDeletedEvent.
    /// </summary>
    /// <param name="loanId">The ID of the deleted loan.</param>
    /// <param name="deletedBy">The user ID who deleted the loan.</param>
    public LoanDeletedEvent(Guid loanId, Guid deletedBy)
    {
        AggregateId = loanId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the loan.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when a Loan has been activated.
/// 
/// **Trigger:** After loan activation
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track loan status changes
/// </summary>
public sealed class LoanActivatedEvent : LoanDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanActivatedEvent.
    /// </summary>
    /// <param name="loanId">The ID of the activated loan.</param>
    public LoanActivatedEvent(Guid loanId)
    {
        AggregateId = loanId;
    }
}

/// <summary>
/// Event raised when a Loan has been deactivated.
/// 
/// **Trigger:** After loan deactivation
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track loan status changes
/// </summary>
public sealed class LoanDeactivatedEvent : LoanDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanDeactivatedEvent.
    /// </summary>
    /// <param name="loanId">The ID of the deactivated loan.</param>
    public LoanDeactivatedEvent(Guid loanId)
    {
        AggregateId = loanId;
    }
}
