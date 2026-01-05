namespace FSH.Module.Microfinance.Events;

/// <summary>
/// Base class for SavingsAccount-related domain events.
/// </summary>
public abstract class SavingsAccountDomainEvent : MicrofinanceDomainEvent
{
    public override string AggregateType => nameof(Domain.SavingsAccount);
}

/// <summary>
/// Event raised when a SavingsAccount has been created.
/// 
/// **Trigger:** After successful SavingsAccount creation
/// **Handlers:** Can send notifications, update search index, log activity
/// **Usage:** Subscribe to track all new savings accounts created in the system
/// </summary>
public sealed class SavingsAccountCreatedEvent : SavingsAccountDomainEvent
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountCreatedEvent.
    /// </summary>
    /// <param name="accountId">The ID of the created savings account.</param>
    /// <param name="name">The name of the created savings account.</param>
    /// <param name="createdBy">The user ID who created the account.</param>
    public SavingsAccountCreatedEvent(Guid accountId, string name, Guid createdBy)
    {
        AggregateId = accountId;
        Name = name;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the name of the created savings account.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who created the account.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a SavingsAccount has been updated.
/// 
/// **Trigger:** After successful SavingsAccount update
/// **Handlers:** Can update search index, send notifications, log changes
/// **Usage:** Track modifications to savings accounts
/// </summary>
public sealed class SavingsAccountUpdatedEvent : SavingsAccountDomainEvent
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountUpdatedEvent.
    /// </summary>
    /// <param name="accountId">The ID of the updated savings account.</param>
    /// <param name="name">The updated name.</param>
    /// <param name="modifiedBy">The user ID who modified the account.</param>
    public SavingsAccountUpdatedEvent(Guid accountId, string name, Guid modifiedBy)
    {
        AggregateId = accountId;
        Name = name;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who modified the account.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a SavingsAccount has been deleted.
/// 
/// **Trigger:** After successful SavingsAccount deletion
/// **Handlers:** Can remove from search index, send notifications, log deletion
/// **Usage:** Track savings account deletions for audit and compliance
/// </summary>
public sealed class SavingsAccountDeletedEvent : SavingsAccountDomainEvent
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountDeletedEvent.
    /// </summary>
    /// <param name="accountId">The ID of the deleted savings account.</param>
    /// <param name="deletedBy">The user ID who deleted the account.</param>
    public SavingsAccountDeletedEvent(Guid accountId, Guid deletedBy)
    {
        AggregateId = accountId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the account.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when a SavingsAccount has been activated.
/// 
/// **Trigger:** After savings account activation
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track account status changes
/// </summary>
public sealed class SavingsAccountActivatedEvent : SavingsAccountDomainEvent
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountActivatedEvent.
    /// </summary>
    /// <param name="accountId">The ID of the activated savings account.</param>
    public SavingsAccountActivatedEvent(Guid accountId)
    {
        AggregateId = accountId;
    }
}

/// <summary>
/// Event raised when a SavingsAccount has been deactivated.
/// 
/// **Trigger:** After savings account deactivation
/// **Handlers:** Can send notifications, update reporting systems, restrict transactions
/// **Usage:** Track account status changes
/// </summary>
public sealed class SavingsAccountDeactivatedEvent : SavingsAccountDomainEvent
{
    /// <summary>
    /// Initializes a new instance of SavingsAccountDeactivatedEvent.
    /// </summary>
    /// <param name="accountId">The ID of the deactivated savings account.</param>
    public SavingsAccountDeactivatedEvent(Guid accountId)
    {
        AggregateId = accountId;
    }
}
