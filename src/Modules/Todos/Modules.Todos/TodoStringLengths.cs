namespace FSH.Modules.Todos;

/// <summary>
/// Defines all string length constants for the Todo module.
/// 
/// **Purpose:**
/// Centralizes all string field length constraints using power-of-2 values (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048, ...).
/// This ensures consistency across domain entities, validators, and database configurations.
/// 
/// **Design Pattern:**
/// - Uses binary-friendly sizes (powers of 2) for memory efficiency and alignment
/// - Single source of truth for all string constraints
/// - Easy to maintain and refactor
/// - Clear naming conventions indicate usage context
/// 
/// **Naming Convention:**
/// {EntityName}{PropertyName}MaxLength
/// Example: TodoNameMaxLength, TodoDescriptionMaxLength
/// 
/// **Why Power of 2?**
/// - Powers of 2 align with memory boundaries
/// - More efficient for string algorithms
/// - Better database index performance
/// - Industry standard for string constraints
/// - Easier to scale and extend
/// 
/// **Usage:**
/// 1. Domain entities: In property definitions
/// 2. Validators: In FluentValidation rules
/// 3. EF Core Configuration: In HasMaxLength() calls
/// 4. Contracts: In DTO properties
/// 5. Documentation: Reference in comments
/// 
/// **String Length Mapping:**
/// - Identifiers/IDs: 64 chars (GUIDs, User IDs, Tenant IDs)
/// - Names/Titles: 128 chars (User names, Todo titles)
/// - Descriptions: 512 chars (Detailed descriptions)
/// - Status/Enum strings: 32 chars (Status, Priority as strings)
/// - Large text: 2048 chars (Notes, detailed content)
/// - Usernames/Email parts: 256 chars (Created/Modified by usernames)
/// </summary>
public static class TodoStringLengths
{
    /// <summary>
    /// Todo entity name/title field maximum length.
    /// 
    /// **Usage:** Todo.Name property
    /// **Validator:** CreateTodoCommandValidator, UpdateTodoCommandValidator
    /// **Database:** TodoConfiguration.Name column
    /// 
    /// **Rationale:** 128 chars allows for descriptive but concise todo titles
    /// Examples: "Complete quarterly budget review", "Schedule team sync meeting", etc.
    /// </summary>
    public const int TodoNameMaxLength = 128;

    /// <summary>
    /// Todo entity description field maximum length.
    /// 
    /// **Usage:** Todo.Description property
    /// **Validator:** CreateTodoCommandValidator, UpdateTodoCommandValidator
    /// **Database:** TodoConfiguration.Description column
    /// 
    /// **Rationale:** 512 chars provides room for detailed descriptions without being excessive
    /// Examples: Multi-line descriptions, requirements, notes, etc.
    /// </summary>
    public const int TodoDescriptionMaxLength = 512;

    /// <summary>
    /// Todo entity notes field maximum length.
    /// 
    /// **Usage:** Todo.Notes property
    /// **Validator:** CreateTodoCommandValidator, UpdateTodoCommandValidator
    /// **Database:** TodoConfiguration.Notes column
    /// 
    /// **Rationale:** 2048 chars allows for extensive notes, references, and additional information
    /// Examples: Detailed notes, links, references, implementation details
    /// </summary>
    public const int TodoNotesMaxLength = 2048;

    /// <summary>
    /// Todo entity status field maximum length (when stored as string).
    /// 
    /// **Usage:** Todo.Status property (as string enum conversion)
    /// **Values:** "NotStarted", "InProgress", "Completed", "OnHold"
    /// **Database:** TodoConfiguration.Status column
    /// 
    /// **Rationale:** 32 chars is sufficient for all status values with room for future extensions
    /// Typical values: "NotStarted" (10), "InProgress" (10), "Completed" (9), "OnHold" (6)
    /// </summary>
    public const int TodoStatusMaxLength = 32;

    /// <summary>
    /// Todo entity tenant ID field maximum length.
    /// 
    /// **Usage:** Todo.TenantId property (inherited from AuditableEntity)
    /// **Database:** TodoConfiguration.TenantId column
    /// 
    /// **Rationale:** 64 chars supports various tenant ID formats (GUIDs, slugs, UUIDs, etc.)
    /// Examples: UUIDs (36 chars), custom tenant IDs, multi-part identifiers
    /// </summary>
    public const int TodoTenantIdMaxLength = 64;

    /// <summary>
    /// Todo entity created by username field maximum length.
    /// 
    /// **Usage:** Todo.CreatedByUserName property (inherited from AuditableEntity)
    /// **Database:** TodoConfiguration.CreatedByUserName column
    /// 
    /// **Rationale:** 256 chars accommodates standard usernames and email addresses
    /// Examples: "john.doe@company.com", "john_doe_123", full display names with spaces
    /// </summary>
    public const int TodoCreatedByUserNameMaxLength = 256;

    /// <summary>
    /// Todo entity last modified by username field maximum length.
    /// 
    /// **Usage:** Todo.LastModifiedByUserName property (inherited from AuditableEntity)
    /// **Database:** TodoConfiguration.LastModifiedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency with CreatedByUserName
    /// Supports same variety of username formats
    /// </summary>
    public const int TodoLastModifiedByUserNameMaxLength = 256;

    // ========== TodoTask String Lengths ==========

    /// <summary>
    /// TodoTask entity name/title field maximum length.
    /// 
    /// **Usage:** TodoTask.Name property
    /// **Validator:** CreateTodoTaskCommandValidator, UpdateTodoTaskCommandValidator
    /// **Database:** TodoTaskConfiguration.Name column
    /// 
    /// **Rationale:** 128 chars allows for descriptive task names
    /// Examples: "Review pull request #123", "Fix login button styling", etc.
    /// </summary>
    public const int TodoTaskNameMaxLength = 128;

    /// <summary>
    /// TodoTask entity description field maximum length.
    /// 
    /// **Usage:** TodoTask.Description property
    /// **Validator:** CreateTodoTaskCommandValidator, UpdateTodoTaskCommandValidator
    /// **Database:** TodoTaskConfiguration.Description column
    /// 
    /// **Rationale:** 512 chars for task-specific details and descriptions
    /// Examples: Task requirements, acceptance criteria, implementation notes
    /// </summary>
    public const int TodoTaskDescriptionMaxLength = 512;

    /// <summary>
    /// TodoTask entity notes field maximum length.
    /// 
    /// **Usage:** TodoTask.Notes property
    /// **Validator:** CreateTodoTaskCommandValidator, UpdateTodoTaskCommandValidator
    /// **Database:** TodoTaskConfiguration.Notes column
    /// 
    /// **Rationale:** 2048 chars for extensive task notes and references
    /// Examples: Detailed implementation notes, links, code references
    /// </summary>
    public const int TodoTaskNotesMaxLength = 2048;

    /// <summary>
    /// TodoTask entity status field maximum length (when stored as string).
    /// 
    /// **Usage:** TodoTask.Status property (as string enum conversion)
    /// **Values:** "Pending", "InProgress", "Completed", "OnHold"
    /// **Database:** TodoTaskConfiguration.Status column
    /// 
    /// **Rationale:** 32 chars is sufficient for task status values
    /// Typical values: "Pending" (7), "InProgress" (10), "Completed" (9), "OnHold" (6)
    /// </summary>
    public const int TodoTaskStatusMaxLength = 32;

    /// <summary>
    /// TodoTask entity tenant ID field maximum length.
    /// 
    /// **Usage:** TodoTask.TenantId property (inherited from AuditableEntity)
    /// **Database:** TodoTaskConfiguration.TenantId column
    /// 
    /// **Rationale:** 64 chars for consistency with Todo.TenantId
    /// </summary>
    public const int TodoTaskTenantIdMaxLength = 64;

    /// <summary>
    /// TodoTask entity created by username field maximum length.
    /// 
    /// **Usage:** TodoTask.CreatedByUserName property (inherited from AuditableEntity)
    /// **Database:** TodoTaskConfiguration.CreatedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency with Todo.CreatedByUserName
    /// </summary>
    public const int TodoTaskCreatedByUserNameMaxLength = 256;

    /// <summary>
    /// TodoTask entity last modified by username field maximum length.
    /// 
    /// **Usage:** TodoTask.LastModifiedByUserName property (inherited from AuditableEntity)
    /// **Database:** TodoTaskConfiguration.LastModifiedByUserName column
    /// 
    /// **Rationale:** 256 chars for consistency with Todo.LastModifiedByUserName
    /// </summary>
    public const int TodoTaskLastModifiedByUserNameMaxLength = 256;

    // ========== Common Constants (Shared) ==========

    /// <summary>
    /// Standard maximum length for email addresses and usernames.
    /// 
    /// **Usage:** User identity properties, email-like identifiers
    /// **Rationale:** 256 chars accommodates full email addresses with extensions
    /// RFC 5321 specifies max 254 chars for local-part + domain
    /// </summary>
    public const int StandardUsernameMaxLength = 256;

    /// <summary>
    /// Standard maximum length for tenant/organization identifiers.
    /// 
    /// **Usage:** Tenant IDs, organization identifiers
    /// **Rationale:** 64 chars supports GUIDs (36), UUIDs, and custom formats
    /// </summary>
    public const int StandardTenantIdMaxLength = 64;

    /// <summary>
    /// Standard maximum length for status enum string representations.
    /// 
    /// **Usage:** Status, priority, and other enum-string conversions
    /// **Rationale:** 32 chars provides room for descriptive status names
    /// Examples: "AwaitingApproval" (15), "CompletedWithErrors" (19)
    /// </summary>
    public const int StandardStatusMaxLength = 32;
}
