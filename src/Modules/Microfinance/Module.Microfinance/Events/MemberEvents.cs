namespace FSH.Module.Microfinance.Events;

/// <summary>
/// Base class for Member-related domain events.
/// </summary>
public abstract class MemberDomainEvent : MicrofinanceDomainEvent
{
    public override string AggregateType => nameof(Domain.Member);
}

/// <summary>
/// Event raised when a Member has been created.
/// 
/// **Trigger:** After successful Member registration
/// **Handlers:** Can send welcome notifications, update search index, log activity
/// **Usage:** Subscribe to track all new members registered in the system
/// </summary>
public sealed class MemberCreatedEvent : MemberDomainEvent
{
    /// <summary>
    /// Initializes a new instance of MemberCreatedEvent.
    /// </summary>
    /// <param name="memberId">The ID of the created member.</param>
    /// <param name="memberNumber">The member number.</param>
    /// <param name="fullName">The full name of the member.</param>
    /// <param name="createdBy">The user ID who created the member.</param>
    public MemberCreatedEvent(Guid memberId, string memberNumber, string fullName, Guid createdBy)
    {
        AggregateId = memberId;
        MemberNumber = memberNumber;
        FullName = fullName;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the member number.
    /// </summary>
    public string MemberNumber { get; }

    /// <summary>
    /// Gets the full name of the created member.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Gets the user ID who created the member.
    /// </summary>
    public Guid CreatedBy { get; }
}

/// <summary>
/// Event raised when a Member has been updated.
/// 
/// **Trigger:** After successful Member profile update
/// **Handlers:** Can update search index, send notifications, log changes
/// **Usage:** Track modifications to member profiles
/// </summary>
public sealed class MemberUpdatedEvent : MemberDomainEvent
{
    /// <summary>
    /// Initializes a new instance of MemberUpdatedEvent.
    /// </summary>
    /// <param name="memberId">The ID of the updated member.</param>
    /// <param name="fullName">The updated full name.</param>
    /// <param name="modifiedBy">The user ID who modified the member.</param>
    public MemberUpdatedEvent(Guid memberId, string fullName, Guid modifiedBy)
    {
        AggregateId = memberId;
        FullName = fullName;
        ModifiedBy = modifiedBy;
    }

    /// <summary>
    /// Gets the updated full name.
    /// </summary>
    public string FullName { get; }

    /// <summary>
    /// Gets the user ID who modified the member.
    /// </summary>
    public Guid ModifiedBy { get; }
}

/// <summary>
/// Event raised when a Member has been deleted.
/// 
/// **Trigger:** After successful Member deletion
/// **Handlers:** Can remove from search index, send notifications, log deletion
/// **Usage:** Track member deletions for audit and compliance
/// </summary>
public sealed class MemberDeletedEvent : MemberDomainEvent
{
    /// <summary>
    /// Initializes a new instance of MemberDeletedEvent.
    /// </summary>
    /// <param name="memberId">The ID of the deleted member.</param>
    /// <param name="deletedBy">The user ID who deleted the member.</param>
    public MemberDeletedEvent(Guid memberId, Guid deletedBy)
    {
        AggregateId = memberId;
        DeletedBy = deletedBy;
    }

    /// <summary>
    /// Gets the user ID who deleted the member.
    /// </summary>
    public Guid DeletedBy { get; }
}

/// <summary>
/// Event raised when a Member has been activated.
/// 
/// **Trigger:** After member account activation
/// **Handlers:** Can send notifications, update reporting systems
/// **Usage:** Track member status changes
/// </summary>
public sealed class MemberActivatedEvent : MemberDomainEvent
{
    /// <summary>
    /// Initializes a new instance of MemberActivatedEvent.
    /// </summary>
    /// <param name="memberId">The ID of the activated member.</param>
    public MemberActivatedEvent(Guid memberId)
    {
        AggregateId = memberId;
    }
}

/// <summary>
/// Event raised when a Member has been deactivated.
/// 
/// **Trigger:** After member account deactivation
/// **Handlers:** Can send notifications, update reporting systems, restrict access
/// **Usage:** Track member status changes
/// </summary>
public sealed class MemberDeactivatedEvent : MemberDomainEvent
{
    /// <summary>
    /// Initializes a new instance of MemberDeactivatedEvent.
    /// </summary>
    /// <param name="memberId">The ID of the deactivated member.</param>
    public MemberDeactivatedEvent(Guid memberId)
    {
        AggregateId = memberId;
    }
}
