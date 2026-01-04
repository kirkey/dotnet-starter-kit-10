namespace FSH.Module.Accounting.Events;

/// <summary>
/// Base class for all Accounting domain events.
/// 
/// **Purpose:**
/// Provides a common base for domain events that track important state changes
/// in the Accounting module. Can be extended to support event sourcing and event streaming.
/// 
/// **Design Pattern:**
/// Implements Domain Event pattern from DDD (Domain-Driven Design).
/// Events represent important business occurrences that have happened.
/// 
/// **Future Extensions:**
/// Can be integrated with:
/// - Event sourcing for audit trails
/// - Event streaming (Kafka, RabbitMQ)
/// - Event-driven microservices
/// - CQRS event store
/// 
/// **Usage:**
/// Domain events are published when important accounting state changes occur.
/// Handlers can subscribe to these events for side effects.
/// </summary>
public abstract class AccountingDomainEvent
{
    /// <summary>
    /// Gets the timestamp when the event occurred.
    /// </summary>
    public DateTimeOffset OccurredAt { get; } = DateTimeOffset.UtcNow;

    /// <summary>
    /// Gets the ID of the aggregate (e.g., JournalEntry, Invoice) that triggered this event.
    /// </summary>
    public Guid AggregateId { get; protected set; }

    /// <summary>
    /// Gets the type of aggregate (for serialization/routing).
    /// </summary>
    public virtual string AggregateType => "Accounting";
}

/// <summary>
/// Event raised when a JournalEntry has been created.
/// 
/// **Trigger:** After successful JournalEntry creation
/// **Handlers:** Can send notifications, validate against rules, log activity
/// **Usage:** Subscribe to track all new journal entries created in the system
/// </summary>
public sealed class JournalEntryCreatedEvent : AccountingDomainEvent
{
    /// <summary>
    /// Initializes a new instance of JournalEntryCreatedEvent.
    /// </summary>
    /// <param name="journalEntryId">The ID of the created journal entry.</param>
    /// <param name="referenceNumber">The reference number of the journal entry.</param>
    /// <param name="createdBy">The user ID who created the journal entry.</param>
    public JournalEntryCreatedEvent(Guid journalEntryId, string referenceNumber, Guid createdBy)
    {
        AggregateId = journalEntryId;
        ReferenceNumber = referenceNumber;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the reference number of the created journal entry.
    /// </summary>
    public string ReferenceNumber { get; }

    /// <summary>
    /// Gets the user ID who created the journal entry.
    /// </summary>
    public Guid CreatedBy { get; }

    /// <summary>
    /// Gets the aggregate type.
    /// </summary>
    public override string AggregateType => nameof(Domain.JournalEntry);
}

/// <summary>
/// Event raised when a JournalEntry has been posted/approved.
/// 
/// **Trigger:** After successful JournalEntry approval
/// **Handlers:** Can update general ledger, update reporting, send notifications
/// **Usage:** Track all posted journal entries (immutable after posting)
/// </summary>
public sealed class JournalEntryPostedEvent : AccountingDomainEvent
{
    /// <summary>
    /// Initializes a new instance of JournalEntryPostedEvent.
    /// </summary>
    /// <param name="journalEntryId">The ID of the posted journal entry.</param>
    /// <param name="referenceNumber">The reference number of the journal entry.</param>
    /// <param name="postedBy">The user ID who posted the journal entry.</param>
    /// <param name="postDate">The date when the entry was posted.</param>
    public JournalEntryPostedEvent(Guid journalEntryId, string referenceNumber, Guid postedBy, DateTimeOffset postDate)
    {
        AggregateId = journalEntryId;
        ReferenceNumber = referenceNumber;
        PostedBy = postedBy;
        PostDate = postDate;
    }

    /// <summary>
    /// Gets the reference number of the posted journal entry.
    /// </summary>
    public string ReferenceNumber { get; }

    /// <summary>
    /// Gets the user ID who posted the journal entry.
    /// </summary>
    public Guid PostedBy { get; }

    /// <summary>
    /// Gets the date when the entry was posted.
    /// </summary>
    public DateTimeOffset PostDate { get; }

    /// <summary>
    /// Gets the aggregate type.
    /// </summary>
    public override string AggregateType => nameof(Domain.JournalEntry);
}

/// <summary>
/// Event raised when an Invoice has been created.
/// 
/// **Trigger:** After successful Invoice creation
/// **Handlers:** Can send notifications, start collection process, update AR
/// **Usage:** Track all new invoices created in the system
/// </summary>
public sealed class InvoiceCreatedEvent : AccountingDomainEvent
{
    /// <summary>
    /// Initializes a new instance of InvoiceCreatedEvent.
    /// </summary>
    /// <param name="invoiceId">The ID of the created invoice.</param>
    /// <param name="invoiceNumber">The invoice number.</param>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="createdBy">The user ID who created the invoice.</param>
    public InvoiceCreatedEvent(Guid invoiceId, string invoiceNumber, Guid customerId, Guid createdBy)
    {
        AggregateId = invoiceId;
        InvoiceNumber = invoiceNumber;
        CustomerId = customerId;
        CreatedBy = createdBy;
    }

    /// <summary>
    /// Gets the invoice number.
    /// </summary>
    public string InvoiceNumber { get; }

    /// <summary>
    /// Gets the customer ID.
    /// </summary>
    public Guid CustomerId { get; }

    /// <summary>
    /// Gets the user ID who created the invoice.
    /// </summary>
    public Guid CreatedBy { get; }

    /// <summary>
    /// Gets the aggregate type.
    /// </summary>
    public override string AggregateType => nameof(Domain.Invoice);
}

/// <summary>
/// Event raised when a FiscalPeriodClose has been completed.
/// 
/// **Trigger:** After successful fiscal period close completion
/// **Handlers:** Can lock period, prevent future changes, generate closing entries
/// **Usage:** Track completed fiscal periods for compliance and reporting
/// </summary>
public sealed class FiscalPeriodClosedEvent : AccountingDomainEvent
{
    /// <summary>
    /// Initializes a new instance of FiscalPeriodClosedEvent.
    /// </summary>
    /// <param name="periodCloseId">The ID of the period close record.</param>
    /// <param name="periodName">The name of the closed period.</param>
    /// <param name="closedBy">The user ID who closed the period.</param>
    public FiscalPeriodClosedEvent(Guid periodCloseId, string periodName, Guid closedBy)
    {
        AggregateId = periodCloseId;
        PeriodName = periodName;
        ClosedBy = closedBy;
    }

    /// <summary>
    /// Gets the name of the closed period.
    /// </summary>
    public string PeriodName { get; }

    /// <summary>
    /// Gets the user ID who closed the period.
    /// </summary>
    public Guid ClosedBy { get; }

    /// <summary>
    /// Gets the aggregate type.
    /// </summary>
    public override string AggregateType => nameof(Domain.FiscalPeriodClose);
}
