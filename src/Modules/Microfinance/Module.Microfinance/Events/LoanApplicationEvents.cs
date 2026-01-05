namespace FSH.Module.Microfinance.Events;

/// <summary>
/// Base class for LoanApplication-related domain events.
/// </summary>
public abstract class LoanApplicationDomainEvent : MicrofinanceDomainEvent
{
    public override string AggregateType => nameof(Domain.LoanApplication);
}

/// <summary>
/// Event raised when a LoanApplication has been created.
/// 
/// **Trigger:** After successful LoanApplication submission
/// **Handlers:** Can send notifications, update search index, log activity, trigger approval workflow
/// **Usage:** Subscribe to track all new loan applications submitted in the system
/// </summary>
public sealed class LoanApplicationCreatedEvent : LoanApplicationDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationCreatedEvent.
    /// </summary>
    /// <param name="applicationId">The ID of the created loan application.</param>
    /// <param name="name">The name of the created loan application.</param>
    /// <param name="createdBy">The user ID who created the application.</param>
    public LoanApplicationCreatedEvent(Guid applicationId, string name, Guid createdBy)
    {
        AggregateId = applicationId;
        Name = name;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the name of the created loan application.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who created the application.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a LoanApplication has been updated.
/// 
/// **Trigger:** After successful LoanApplication update
/// **Handlers:** Can update search index, send notifications, log changes
/// **Usage:** Track modifications to loan applications
/// </summary>
public sealed class LoanApplicationUpdatedEvent : LoanApplicationDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationUpdatedEvent.
    /// </summary>
    /// <param name="applicationId">The ID of the updated loan application.</param>
    /// <param name="name">The updated name.</param>
    /// <param name="modifiedBy">The user ID who modified the application.</param>
    public LoanApplicationUpdatedEvent(Guid applicationId, string name, Guid modifiedBy)
    {
        AggregateId = applicationId;
        Name = name;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated name.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets the user ID who modified the application.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a LoanApplication has been deleted.
/// 
/// **Trigger:** After successful LoanApplication deletion
/// **Handlers:** Can remove from search index, send notifications, log deletion
/// **Usage:** Track loan application deletions for audit and compliance
/// </summary>
public sealed class LoanApplicationDeletedEvent : LoanApplicationDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationDeletedEvent.
    /// </summary>
    /// <param name="applicationId">The ID of the deleted loan application.</param>
    /// <param name="deletedBy">The user ID who deleted the application.</param>
    public LoanApplicationDeletedEvent(Guid applicationId, Guid deletedBy)
    {
        AggregateId = applicationId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the application.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when a LoanApplication has been activated.
/// 
/// **Trigger:** After loan application activation
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track application status changes
/// </summary>
public sealed class LoanApplicationActivatedEvent : LoanApplicationDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationActivatedEvent.
    /// </summary>
    /// <param name="applicationId">The ID of the activated loan application.</param>
    public LoanApplicationActivatedEvent(Guid applicationId)
    {
        AggregateId = applicationId;
    }
}

/// <summary>
/// Event raised when a LoanApplication has been deactivated.
/// 
/// **Trigger:** After loan application deactivation or rejection
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track application status changes
/// </summary>
public sealed class LoanApplicationDeactivatedEvent : LoanApplicationDomainEvent
{
    /// <summary>
    /// Initializes a new instance of LoanApplicationDeactivatedEvent.
    /// </summary>
    /// <param name="applicationId">The ID of the deactivated loan application.</param>
    public LoanApplicationDeactivatedEvent(Guid applicationId)
    {
        AggregateId = applicationId;
    }
}
