# Copilot API Module Instructions - FSH .NET 10 Starter Kit

> **IMPORTANT**: This document extends `COPILOT_INSTRUCTIONS.md` with practical, tested patterns from the **Todo Module** implementation. Use this as a reference for API module development.

## 📋 Quick Checklist for New API Modules

- [ ] Module structure follows the directory pattern (Contracts + Implementation)
- [ ] String lengths use property-specific constants (e.g., TodoNameMaxLength, TodoDescriptionMaxLength)
- [ ] Constants are powers of 2 for database performance (128, 256, 512, 1024, 2048)
- [ ] Centralized exception classes with proper inheritance
- [ ] Entity factory methods for aggregate creation
- [ ] Domain model methods for state transitions
- [ ] Feature structure: Command/Query → Validator → Handler → Endpoint
- [ ] Validation uses extension methods for DRY code
- [ ] Handlers use ICommandHandler/IQueryHandler from Mediator library
- [ ] Endpoints are static extension methods on IEndpointRouteBuilder
- [ ] Multi-tenancy integrated (TenantId, ICurrentUser)
- [ ] Audit trail tracking (CreatedByUserName, LastModifiedByUserName, timestamps)
- [ ] Module.cs implements IModule with ConfigureServices + MapEndpoints
- [ ] Permission constants defined for all features
- [ ] Global usings file (GlobalUsings.cs) configured
- [ ] DbContext properly configured with multi-tenancy
- [ ] Tests cover happy path and edge cases

---

## 🏗️ Todo Module: Reference Implementation

The **Todo Module** is the reference implementation for all new API modules. It demonstrates:
- Master-Detail relationship (Todo → TodoTask)
- State machine pattern (NotStarted → InProgress → Completed → OnHold)
- Bulk operations (Import/Export)
- Soft deletes (Archive/Restore)
- Complete exception handling
- Power-of-2 string length constants
- Comprehensive documentation

### Key Files to Study

```
src/Modules/Todos/
├── Modules.Todos.Contracts/
│   └── v1/Todos/             # CreateTodoCommand, GetTodosQuery, TodoResponse
├── Modules.Todos/
│   ├── TodoModule.cs         # Module registration example
│   ├── TodoStringLengths.cs  # String length constants
│   ├── TodoPermissionConstants.cs  # Feature permissions
│   ├── Domain/               # Entity examples
│   │   ├── Todo.cs           # Master aggregate
│   │   ├── TodoTask.cs       # Detail entity
│   │   └── TodoEnums.cs      # Status/Priority enums
│   ├── Exceptions/           # Centralized exceptions
│   ├── Features/v1/Todos/    # Feature examples
│   │   ├── CreateTodo/       # Command pattern
│   │   ├── GetTodos/         # Query pattern
│   │   └── ArchiveTodo/      # State transition
│   └── Data/
│       ├── TodoDbContext.cs  # Multi-tenant DbContext
│       └── Configurations/   # EF Core configurations
```

---

## 📐 Complete Module Structure

### 1. String Length Constants (Property-Specific)

**File**: `Modules.{Module}/{Module}StringLengths.cs`

```csharp
namespace FSH.Modules.{Module};

/// <summary>
/// Centralized string length constants for the {Module} module.
/// 
/// **Design Principle:**
/// All string lengths are powers of 2 (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048)
/// for optimal database performance, memory alignment, and scalability.
/// 
/// **Naming Convention:**
/// {EntityName}{PropertyName}MaxLength
/// Example: TodoNameMaxLength, TodoDescriptionMaxLength
/// 
/// **Usage:**
/// - Domain entities (property definitions)
/// - FluentValidation rules
/// - Entity configurations (HasMaxLength)
/// - Contracts/DTOs for consistency
/// 
/// **Why Property-Specific Constants?**
/// - Clear intent: Name vs. Description vs. Notes
/// - Single source of truth per field
/// - Easy to audit and maintain
/// - Supports different lengths for different entities
/// </summary>
public static class {Module}StringLengths
{
    // ========== {Entity} String Lengths ==========
    
    /// <summary>
    /// {Entity} name/title field maximum length.
    /// 
    /// **Usage:** {Entity}.Name property
    /// **Rationale:** 128 chars allows for descriptive but concise titles
    /// </summary>
    public const int {Entity}NameMaxLength = 128;
    
    /// <summary>
    /// {Entity} description field maximum length.
    /// 
    /// **Usage:** {Entity}.Description property
    /// **Rationale:** 512 chars provides room for detailed descriptions
    /// </summary>
    public const int {Entity}DescriptionMaxLength = 512;
    
    /// <summary>
    /// {Entity} notes field maximum length.
    /// 
    /// **Usage:** {Entity}.Notes property
    /// **Rationale:** 2048 chars allows for extensive notes and references
    /// </summary>
    public const int {Entity}NotesMaxLength = 2048;
    
    /// <summary>
    /// {Entity} status field maximum length (when stored as string).
    /// 
    /// **Usage:** {Entity}.Status property (enum string conversion)
    /// **Rationale:** 32 chars is sufficient for all status values
    /// </summary>
    public const int {Entity}StatusMaxLength = 32;
    
    /// <summary>
    /// {Entity} tenant ID field maximum length.
    /// 
    /// **Usage:** {Entity}.TenantId property
    /// **Rationale:** 64 chars supports various tenant ID formats
    /// </summary>
    public const int {Entity}TenantIdMaxLength = 64;
    
    /// <summary>
    /// {Entity} created by username field maximum length.
    /// 
    /// **Usage:** {Entity}.CreatedByUserName property
    /// **Rationale:** 256 chars accommodates usernames and email addresses
    /// </summary>
    public const int {Entity}CreatedByUserNameMaxLength = 256;
    
    /// <summary>
    /// {Entity} last modified by username field maximum length.
    /// 
    /// **Usage:** {Entity}.LastModifiedByUserName property
    /// **Rationale:** 256 chars for consistency
    /// </summary>
    public const int {Entity}LastModifiedByUserNameMaxLength = 256;
    
    // ========== Common Constants (Shared) ==========
    
    /// <summary>Standard maximum length for status enum string representations.</summary>
    public const int StandardStatusMaxLength = 32;
    
    /// <summary>Standard maximum length for tenant/organization identifiers.</summary>
    public const int StandardTenantIdMaxLength = 64;
    
    /// <summary>Standard maximum length for email addresses and usernames.</summary>
    public const int StandardUsernameMaxLength = 256;
}
```

**Usage Examples**:

```csharp
// In domain entity
public class Todo : AuditableEntity<Guid>
{
    public string Name { get; private set; } = default!;          // 128 chars
    public string? Description { get; private set; };            // 512 chars
    public string? Notes { get; private set; };                  // 2048 chars
}

// In FluentValidation
public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(TodoStringLengths.TodoNameMaxLength);
        
        RuleFor(x => x.Description)
            .MaximumLength(TodoStringLengths.TodoDescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));
    }
}

// In entity configuration
public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoNameMaxLength);
    }
}
```

---

### 2. Exception Handling (Centralized)

**File**: `Modules.{Module}/Exceptions/{Entity}Exceptions.cs`

```csharp
namespace FSH.Modules.{Module}.Exceptions;

/// <summary>
/// Thrown when a todo item is not found in the database.
/// </summary>
public class TodoNotFoundException : NotFoundException
{
    public TodoNotFoundException(Guid id)
        : base($"Todo with ID '{id}' was not found.")
    {
    }
}

/// <summary>
/// Thrown when attempting to perform an operation on a non-existent parent todo.
/// </summary>
public class ParentTodoNotFoundException : NotFoundException
{
    public ParentTodoNotFoundException(Guid todoId)
        : base($"Parent todo with ID '{todoId}' was not found.")
    {
    }
}

/// <summary>
/// Thrown when a todo task is not found.
/// </summary>
public class TodoTaskNotFoundException : NotFoundException
{
    public TodoTaskNotFoundException(Guid taskId)
        : base($"Todo task with ID '{taskId}' was not found.")
    {
    }
}

/// <summary>
/// Extension methods for common exception scenarios in Todo operations.
/// Centralizes exception creation to avoid duplication across handlers.
/// </summary>
public static class TodoExceptionExtensions
{
    /// <summary>
    /// Throws TodoNotFoundException if entity is null.
    /// </summary>
    public static Todo ThrowIfNotFound(this Todo? todo, Guid id)
        => todo ?? throw new TodoNotFoundException(id);
    
    /// <summary>
    /// Throws TodoTaskNotFoundException if entity is null.
    /// </summary>
    public static TodoTask ThrowIfNotFound(this TodoTask? task, Guid id)
        => task ?? throw new TodoTaskNotFoundException(id);
    
    /// <summary>
    /// Throws ParentTodoNotFoundException if parent todo is null.
    /// </summary>
    public static Todo ThrowIfParentNotFound(this Todo? todo, Guid parentId)
        => todo ?? throw new ParentTodoNotFoundException(parentId);
}
```

**Usage in Handlers**:

```csharp
public sealed class DeleteTodoCommandHandler(TodoDbContext context)
    : ICommandHandler<DeleteTodoCommand, Unit>
{
    public async ValueTask<Unit> Handle(DeleteTodoCommand command, CancellationToken ct)
    {
        var todo = await context.Todos.FindAsync(new object[] { command.Id }, cancellationToken: ct)
            ?? throw new TodoNotFoundException(command.Id);
        
        context.Todos.Remove(todo);
        await context.SaveChangesAsync(ct);
        
        return Unit.Value;
    }
}

// Or using extension method
var todo = await context.Todos.FindAsync(new object[] { command.Id }, cancellationToken: ct);
todo.ThrowIfNotFound(command.Id);
```

---

### 3. Domain Entity Pattern

**File**: `Modules.{Module}/Domain/{Entity}.cs`

```csharp
using FSH.Framework.Core.Domain;

namespace FSH.Modules.{Module}.Domain;

/// <summary>
/// Represents a {entity description}.
/// 
/// **Purpose:**
/// {Describe the business purpose and responsibilities}
/// 
/// **Business Logic:**
/// - {Key business rule 1}
/// - {Key business rule 2}
/// - {Key business rule 3}
/// 
/// **Relationships:**
/// - {Navigation properties and cardinalities}
/// 
/// **Multi-Tenancy:**
/// - Supports multi-tenancy through TenantId property
/// 
/// **Audit Trail:**
/// - Tracks creation and modification details
/// - Supports soft deletes through IsActive flag
/// </summary>
public class Todo : AuditableEntity<Guid>
{
    /// <summary>Gets or sets the priority level of this todo.</summary>
    public TodoPriority Priority { get; private set; } = TodoPriority.Medium;
    
    /// <summary>Gets or sets the due date for this todo (nullable, UTC).</summary>
    public DateTimeOffset? DueDate { get; private set; }
    
    /// <summary>Gets a value indicating whether this todo has been completed.</summary>
    public bool IsCompleted { get; private set; }
    
    /// <summary>Gets the timestamp when this todo was completed.</summary>
    public DateTimeOffset? CompletedAt { get; private set; }
    
    /// <summary>Gets the collection of TodoTask items associated with this todo.</summary>
    public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();
    
    private Todo() { } // EF Core
    
    /// <summary>
    /// Factory method to create a new Todo item.
    /// 
    /// Initializes a new todo with default values:
    /// - Status: NotStarted
    /// - IsActive: true
    /// - IsCompleted: false
    /// - Priority: Medium (if not specified)
    /// </summary>
    /// <param name="name">The name/title (required, cannot be null or whitespace).</param>
    /// <param name="tenantId">The tenant ID for multi-tenancy support.</param>
    /// <param name="createdBy">The user ID of the creator.</param>
    /// <param name="createdByUserName">The username of the creator.</param>
    /// <param name="description">Optional description.</param>
    /// <param name="priority">Priority level (defaults to Medium).</param>
    /// <param name="dueDate">Optional due date (converted to UTC).</param>
    /// <returns>A new Todo instance with all properties initialized.</returns>
    /// <exception cref="ArgumentException">Thrown when name or tenantId is null/whitespace.</exception>
    public static Todo Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null,
        TodoPriority priority = TodoPriority.Medium,
        DateTimeOffset? dueDate = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
        
        return new Todo
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Priority = priority,
            DueDate = dueDate?.ToUniversalTime(),
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow,
            Status = nameof(TodoStatus.NotStarted),
            IsActive = true,
            IsCompleted = false
        };
    }
    
    /// <summary>
    /// Updates the details of this todo.
    /// 
    /// Updates name, description, priority, and due date.
    /// Automatically records modification details.
    /// </summary>
    /// <param name="name">New name (required, cannot be null/whitespace).</param>
    /// <param name="description">New description (optional).</param>
    /// <param name="priority">New priority level.</param>
    /// <param name="dueDate">New due date (converted to UTC, can be null).</param>
    /// <param name="modifiedBy">User ID making the modification.</param>
    /// <param name="modifiedByUserName">Username making the modification.</param>
    public void Update(
        string name,
        string? description,
        TodoPriority priority,
        DateTimeOffset? dueDate,
        Guid modifiedBy,
        string modifiedByUserName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        
        Name = name;
        Description = description;
        Priority = priority;
        DueDate = dueDate?.ToUniversalTime();
        SetModifiedBy(modifiedBy, modifiedByUserName);
    }
    
    /// <summary>
    /// Marks this todo as completed.
    /// 
    /// Sets IsCompleted to true, records completion timestamp, and updates status to Completed.
    /// </summary>
    public void Complete()
    {
        IsCompleted = true;
        CompletedAt = DateTimeOffset.UtcNow;
        Status = nameof(TodoStatus.Completed);
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>
    /// Reopens a previously completed todo.
    /// 
    /// Sets IsCompleted to false, clears completion timestamp, updates status to InProgress.
    /// </summary>
    public void Reopen()
    {
        IsCompleted = false;
        CompletedAt = null;
        Status = nameof(TodoStatus.InProgress);
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>
    /// Updates the status of this todo.
    /// 
    /// Changes status and automatically marks as completed if status is Completed.
    /// </summary>
    public void UpdateStatus(TodoStatus status)
    {
        Status = status.ToString();
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
        
        if (status == TodoStatus.Completed && !IsCompleted)
        {
            Complete();
        }
    }
    
    /// <summary>Archives this todo (soft delete).</summary>
    public void Archive()
    {
        IsActive = false;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
    
    /// <summary>Restores a previously archived todo.</summary>
    public void Restore()
    {
        IsActive = true;
        LastModifiedOnUtc = DateTimeOffset.UtcNow;
    }
}
```

**Key Principles**:
1. **Factory Method (Create)**: Use static factory methods instead of constructor for complex initialization
2. **Private Setters**: Properties are private setters to enforce business logic
3. **Domain Methods**: State transitions (Complete, Reopen, UpdateStatus) encapsulate business logic
4. **Validation**: Factory method validates required inputs
5. **Audit Trail**: Tracks CreatedBy, CreatedOnUtc, LastModifiedBy, LastModifiedOnUtc
6. **Multi-Tenancy**: Always includes TenantId property
7. **UTC Conversion**: All DateTimeOffset properties convert to UTC for consistency
8. **Documentation**: Comprehensive XML comments explaining purpose, parameters, and exceptions

---

### 4. Enums and Constants

**File**: `Modules.{Module}/Domain/{Entity}Enums.cs`

```csharp
namespace FSH.Modules.{Module}.Domain;

/// <summary>
/// Represents the priority levels for a todo item.
/// 
/// Used to determine the urgency and execution order of todos.
/// </summary>
public enum TodoPriority
{
    /// <summary>No specific priority (0).</summary>
    None = 0,
    
    /// <summary>Low priority (1).</summary>
    Low = 1,
    
    /// <summary>Medium priority (2).</summary>
    Medium = 2,
    
    /// <summary>High priority (3).</summary>
    High = 3,
    
    /// <summary>Critical priority (4) - highest urgency.</summary>
    Critical = 4
}

/// <summary>
/// Represents the workflow status of a todo item.
/// 
/// Defines the lifecycle states a todo can be in:
/// NotStarted → InProgress → OnHold → Completed
/// 
/// Todos can be reopened from Completed back to InProgress.
/// </summary>
public enum TodoStatus
{
    /// <summary>Todo has not been started yet.</summary>
    NotStarted = 0,
    
    /// <summary>Todo is currently being worked on.</summary>
    InProgress = 1,
    
    /// <summary>Todo has been completed successfully.</summary>
    Completed = 2,
    
    /// <summary>Todo is temporarily on hold pending some action.</summary>
    OnHold = 3
}
```

---

### 5. Permission Constants

**File**: `Modules.{Module}/{Module}PermissionConstants.cs`

```csharp
namespace FSH.Modules.{Module};

/// <summary>
/// Centralized permission constants for the Todo module.
/// 
/// **Pattern**: {module}:{feature}:{action}
/// Example: todos:todos:create
/// 
/// Permissions are registered in TodoModule.ConfigureServices()
/// and enforced on endpoints using RequirePermission()
/// </summary>
public static class TodoPermissionConstants
{
    /// <summary>Permission prefix for all todo module permissions.</summary>
    private const string Prefix = "todos";
    
    /// <summary>Permissions for todo management.</summary>
    public static class Todos
    {
        /// <summary>Permission to view todo items: todos:todos:view</summary>
        public const string View = $"{Prefix}:todos:view";
        
        /// <summary>Permission to search todo items: todos:todos:search</summary>
        public const string Search = $"{Prefix}:todos:search";
        
        /// <summary>Permission to create new todo items: todos:todos:create</summary>
        public const string Create = $"{Prefix}:todos:create";
        
        /// <summary>Permission to update existing todo items: todos:todos:update</summary>
        public const string Update = $"{Prefix}:todos:update";
        
        /// <summary>Permission to delete todo items: todos:todos:delete</summary>
        public const string Delete = $"{Prefix}:todos:delete";
        
        /// <summary>Permission to export todos: todos:todos:export</summary>
        public const string Export = $"{Prefix}:todos:export";
        
        /// <summary>Permission to import todos: todos:todos:import</summary>
        public const string Import = $"{Prefix}:todos:import";
    }
    
    /// <summary>
    /// Returns all permissions for the Todo module.
    /// 
    /// Called in TodoModule.ConfigureServices() to register all permissions.
    /// </summary>
    public static IReadOnlyList<string> GetPermissions()
    {
        return new List<string>
        {
            Todos.View,
            Todos.Search,
            Todos.Create,
            Todos.Update,
            Todos.Delete,
            Todos.Export,
            Todos.Import
        };
    }
}
```

**Usage in Module**:

```csharp
public class TodoModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register all permissions
        PermissionConstants.Register(TodoPermissionConstants.GetPermissions());
        // ... rest of configuration
    }
}
```

---

### 6. DbContext with Multi-Tenancy

**File**: `Modules.{Module}/Data/{Module}DbContext.cs`

```csharp
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Eventing.Inbox;
using FSH.Framework.Eventing.Outbox;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Todos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FSH.Modules.Todos.Data;

/// <summary>
/// Entity Framework Core DbContext for the Todo module.
/// 
/// **Features:**
/// - Multi-tenancy support via Finbuckle
/// - Automatic tenant connection string resolution
/// - Event Sourcing support (Outbox/Inbox pattern)
/// - Health check integration
/// 
/// **Tenant Isolation:**
/// Automatically routes queries to tenant-specific databases based on current tenant context.
/// </summary>
public class TodoDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;
    
    /// <summary>Gets or sets the collection of Todo entities.</summary>
    public DbSet<Todo> Todos => Set<Todo>();
    
    /// <summary>Gets or sets the collection of TodoTask entities.</summary>
    public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
    
    /// <summary>Gets or sets the collection of outbox messages for event sourcing.</summary>
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    
    /// <summary>Gets or sets the collection of inbox messages for event sourcing.</summary>
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    
    public TodoDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<TodoDbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment) : base(options)
    {
        _environment = environment;
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }
    
    /// <summary>
    /// Configures the model using Entity Framework conventions and configurations.
    /// 
    /// Applies all entity configurations from the assembly and sets up Outbox/Inbox messaging.
    /// </summary>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply all entity configurations from this assembly
        builder.ApplyConfigurationsFromAssembly(typeof(TodoDbContext).Assembly);
        
        // Configure Outbox/Inbox for event-driven architecture
        builder.ApplyConfiguration(new OutboxMessageConfiguration("todos"));
        builder.ApplyConfiguration(new InboxMessageConfiguration("todos"));
    }
    
    /// <summary>
    /// Configures the database connection based on current tenant context.
    /// 
    /// Resolves the tenant-specific connection string and configures the appropriate database provider.
    /// </summary>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.ConfigureHeroDatabase(
                _settings.Provider,
                TenantInfo.ConnectionString,
                _settings.MigrationsAssembly,
                _environment.IsDevelopment());
        }
    }
}
```

---

### 7. Entity Configuration (EF Core)

**File**: `Modules.{Module}/Data/Configurations/{Entity}Configuration.cs`

```csharp
using FSH.Modules.Todos.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Todos.Data.Configurations;

/// <summary>
/// Entity Framework Core configuration for the Todo entity.
/// 
/// **Responsibilities:**
/// - Maps domain entity to database table
/// - Configures property constraints (MaxLength, Required, etc.)
/// - Defines relationships and foreign keys
/// - Creates indexes for query performance
/// </summary>
public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        // Table mapping to 'todos' schema
        builder.ToTable("Todos", "todos");
        
        // Primary key
        builder.HasKey(x => x.Id);
        
        // Required properties with string length constraints using property-specific constants
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoNameMaxLength);
        
        builder.Property(x => x.Description)
            .HasMaxLength(TodoStringLengths.TodoDescriptionMaxLength);
        
        builder.Property(x => x.Notes)
            .HasMaxLength(TodoStringLengths.TodoNotesMaxLength);
        
        builder.Property(x => x.Status)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoStatusMaxLength);
        
        // Enum properties
        builder.Property(x => x.Priority)
            .HasConversion<int>();
        
        // Audit properties
        builder.Property(x => x.TenantId)
            .IsRequired()
            .HasMaxLength(TodoStringLengths.TodoTenantIdMaxLength);
        
        builder.Property(x => x.CreatedByUserName)
            .HasMaxLength(TodoStringLengths.TodoCreatedByUserNameMaxLength);
        
        builder.Property(x => x.LastModifiedByUserName)
            .HasMaxLength(TodoStringLengths.TodoLastModifiedByUserNameMaxLength);
        
        // Indexes for query performance
        builder.HasIndex(x => x.TenantId)
            .HasDatabaseName("IX_Todos_TenantId");
        
        builder.HasIndex(x => new { x.TenantId, x.IsActive })
            .HasDatabaseName("IX_Todos_TenantId_IsActive");
        
        builder.HasIndex(x => new { x.TenantId, x.Status })
            .HasDatabaseName("IX_Todos_TenantId_Status");
        
        builder.HasIndex(x => x.CreatedOnUtc)
            .HasDatabaseName("IX_Todos_CreatedOnUtc");
        
        // Relationships
        builder.HasMany(x => x.Tasks)
            .WithOne(t => t.Todo)
            .HasForeignKey(t => t.TodoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

---

### 8. Validation Extensions (DRY Validation Rules)

**File**: `Modules.{Module}/Features/v1/{Feature}/{Feature}ValidationExtensions.cs`

```csharp
using FluentValidation;
using FSH.Modules.Todos.Contracts.v1.Todos;

namespace FSH.Modules.Todos.Features.v1.Todos;

/// <summary>
/// Extension methods for FluentValidation rules used across Todo features.
/// 
/// **Purpose:**
/// Centralizes common validation rules to avoid duplication across multiple validators.
/// All rules reference property-specific string length constants for consistency.
/// 
/// **Benefits:**
/// - DRY principle: Define once, use everywhere
/// - Consistency: All validators use the same rules
/// - Maintainability: Update one place, applies everywhere
/// - Reusability: Easily compose validators
/// </summary>
public static class TodoValidationExtensions
{
    /// <summary>
    /// Validates todo name: Required, max length using TodoNameMaxLength.
    /// </summary>
    public static IRuleBuilderOptions<T, string> ValidateTodoName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Todo name is required")
            .MaximumLength(TodoStringLengths.TodoNameMaxLength)
            .WithMessage($"Todo name cannot exceed {TodoStringLengths.TodoNameMaxLength} characters");
    }
    
    /// <summary>
    /// Validates todo description: Optional, max length using TodoDescriptionMaxLength.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateTodoDescription<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoDescriptionMaxLength)
            .WithMessage($"Description cannot exceed {TodoStringLengths.TodoDescriptionMaxLength} characters")
            .When(x => !string.IsNullOrEmpty(x));
    }
    
    /// <summary>
    /// Validates todo notes: Optional, max length using TodoNotesMaxLength.
    /// </summary>
    public static IRuleBuilderOptions<T, string?> ValidateTodoNotes<T>(
        this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(TodoStringLengths.TodoNotesMaxLength)
            .WithMessage($"Notes cannot exceed {TodoStringLengths.TodoNotesMaxLength} characters")
            .When(x => !string.IsNullOrEmpty(x));
    }
    
    /// <summary>
    /// Validates priority: Required, valid enum value.
    /// </summary>
    public static IRuleBuilderOptions<T, int> ValidatePriority<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .GreaterThanOrEqualTo((int)TodoPriority.None)
            .WithMessage("Invalid priority level")
            .LessThanOrEqualTo((int)TodoPriority.Critical)
            .WithMessage("Priority must be between None and Critical");
    }
}
```

**Usage in Validators**:

```csharp
public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        // Uses extension methods instead of duplicating validation logic
        RuleFor(x => x.Name).ValidateTodoName();
        RuleFor(x => x.Description).ValidateTodoDescription();
        RuleFor(x => x.Notes).ValidateTodoNotes();
        RuleFor(x => x.Priority).ValidatePriority();
    }
}
```

---

### 9. Feature: Command Handler & Validator

**File**: `Modules.{Module}/Features/v1/{Feature}/{Feature}CommandHandler.cs`

```csharp
using FSH.Framework.Core.Context;
using FSH.Modules.Todos.Contracts.v1.Todos;
using FSH.Modules.Todos.Data;
using FSH.Modules.Todos.Domain;
using Mediator;

namespace FSH.Modules.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Handles the creation of a new todo item.
/// 
/// **Responsibilities:**
/// 1. Create a new Todo aggregate using the factory method
/// 2. Set additional properties from command
/// 3. Persist to database
/// 4. Return the new todo's ID
/// 
/// **Domain Logic:**
/// - Uses Todo.Create factory method for proper initialization
/// - Converts priority from command enum value
/// - Associates todo with current user and tenant
/// - Maintains audit trail through creation tracking
/// 
/// **Dependencies:**
/// - TodoDbContext: For database persistence
/// - ICurrentUser: For accessing current user context (ID, username, tenant)
/// </summary>
public sealed class CreateTodoCommandHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : ICommandHandler<CreateTodoCommand, Guid>
{
    /// <summary>
    /// Handles the CreateTodoCommand to create a new todo item.
    /// </summary>
    /// <param name="command">The command containing todo creation details.</param>
    /// <param name="cancellationToken">Cancellation token for the operation.</param>
    /// <returns>The ID of the newly created todo.</returns>
    public async ValueTask<Guid> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        // 1. Create todo using domain factory method
        var todo = Domain.Todo.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description,
            (TodoPriority)command.Priority,
            command.DueDate);
        
        // 2. Set additional properties
        if (!string.IsNullOrWhiteSpace(command.Notes))
        {
            todo.Notes = command.Notes;
        }
        
        // 3. Persist to database
        context.Todos.Add(todo);
        await context.SaveChangesAsync(cancellationToken);
        
        // 4. Return created ID
        return todo.Id;
    }
}
```

**File**: `Modules.{Module}/Features/v1/{Feature}/{Feature}CommandValidator.cs`

```csharp
using FluentValidation;
using FSH.Modules.Todos.Contracts.v1.Todos;

namespace FSH.Modules.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Validator for CreateTodoCommand.
/// 
/// **Purpose:**
/// Validates the CreateTodoCommand request using FluentValidation.
/// All validation rules use constants and extension methods for consistency.
/// 
/// **Validation Rules:**
/// - Name: Required, max 128 chars (using TodoStringLengths.XLarge)
/// - Description: Optional, max 256 chars
/// - Notes: Optional, max 256 chars
/// - Priority: Required, valid enum (0-4)
/// - DueDate: Optional, must be in future if provided
/// 
/// **DRY Pattern:**
/// Uses TodoValidationExtensions to avoid duplicating rules across validators.
/// </summary>
public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        // Use extension methods for DRY code
        RuleFor(x => x.Name).ValidateTodoName();
        RuleFor(x => x.Description).ValidateTodoDescription();
        RuleFor(x => x.Notes).ValidateTodoNotes();
        RuleFor(x => x.Priority).ValidatePriority();
        
        // Custom rule: Due date must be in future
        RuleFor(x => x.DueDate)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Due date must be in the future")
            .When(x => x.DueDate.HasValue);
    }
}
```

---

### 10. Feature: Endpoint

**File**: `Modules.{Module}/Features/v1/{Feature}/{Feature}Endpoint.cs`

```csharp
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Todos.Contracts.v1.Todos;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Todos.Features.v1.Todos.CreateTodo;

/// <summary>
/// Endpoint for creating a new todo item.
/// 
/// **HTTP Mapping:**
/// POST /api/v1/todo
/// 
/// **Purpose:**
/// Provides the HTTP endpoint for creating a new todo item with the provided details.
/// 
/// **Security:**
/// Requires TodoPermissionConstants.Todos.Create permission.
/// 
/// **Request Body:**
/// {
///   "name": "string",
///   "description": "string",
///   "notes": "string",
///   "priority": 0-4,
///   "dueDate": "2024-12-31T00:00:00Z"
/// }
/// 
/// **Response:**
/// - Status 201 Created
/// - Location: /api/v1/todo/{id}
/// - Body: The newly created todo ID (Guid)
/// 
/// **Validation Errors:**
/// - Status 400 Bad Request
/// - Body: Validation problem details with error messages for each field
/// </summary>
public static class CreateTodoEndpoint
{
    /// <summary>
    /// Maps the CreateTodo endpoint to the route group.
    /// </summary>
    /// <param name="endpoints">The endpoint route builder to configure.</param>
    /// <returns>A route handler builder for further configuration.</returns>
    public static RouteHandlerBuilder MapCreateTodoEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateTodoCommand command,
            IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var id = await mediator.Send(command, cancellationToken);
            return TypedResults.Created($"/api/v1/todo/{id}", id);
        })
        .WithName(nameof(CreateTodoEndpoint))
        .WithSummary("Create a new todo item")
        .WithDescription("Creates a new todo item with the provided details. Returns the ID of the created todo.")
        .RequirePermission(TodoPermissionConstants.Todos.Create)
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem(StatusCodes.Status400BadRequest)
        .WithOpenApi();
    }
}
```

**Key Points**:
1. **Static Extension Method**: `Map{Feature}Endpoint` extends `IEndpointRouteBuilder`
2. **Route Naming**: `.WithName()` should match class name
3. **Permissions**: Always use `.RequirePermission()` for authorization
4. **Return Types**: Use `TypedResults` for type safety
5. **Documentation**: Include `.WithSummary()`, `.WithDescription()`, `.WithOpenApi()`
6. **Status Codes**: Specify all possible responses with `.Produces()`

---

### 11. Feature: Query Handler

**File**: `Modules.{Module}/Features/v1/{Feature}/{Feature}QueryHandler.cs`

```csharp
using FSH.Framework.Core.Context;
using FSH.Modules.Todos.Contracts.v1.Todos;
using FSH.Modules.Todos.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Todos.Features.v1.Todos.GetTodos;

/// <summary>
/// Handles retrieving paginated list of todos with filtering and sorting.
/// 
/// **Responsibilities:**
/// 1. Query todos for the current tenant
/// 2. Apply search/filter criteria
/// 3. Apply sorting
/// 4. Apply pagination
/// 5. Project to response DTOs
/// 
/// **Performance Considerations:**
/// - Uses .AsNoTracking() for read-only queries
/// - Applies pagination at database level
/// - Projects to DTO before materializing results
/// - Uses indexes on TenantId, Status, CreatedOnUtc
/// 
/// **Multi-Tenancy:**
/// Automatically filters by current tenant via ICurrentUser context.
/// </summary>
public sealed class GetTodosQueryHandler(
    TodoDbContext context,
    ICurrentUser currentUser)
    : IQueryHandler<GetTodosQuery, PagedList<TodoResponse>>
{
    public async ValueTask<PagedList<TodoResponse>> Handle(
        GetTodosQuery query,
        CancellationToken cancellationToken)
    {
        // Start query - read-only, no tracking
        var todoQuery = context.Todos.AsNoTracking()
            .Where(t => t.TenantId == currentUser.GetTenant());
        
        // Apply search filter
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var searchTerm = query.Search.Trim().ToLower();
            todoQuery = todoQuery.Where(t =>
                t.Name.ToLower().Contains(searchTerm) ||
                t.Description!.ToLower().Contains(searchTerm));
        }
        
        // Apply status filter
        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            todoQuery = todoQuery.Where(t => t.Status == query.Status);
        }
        
        // Apply sorting
        todoQuery = query.OrderBy switch
        {
            "name" => todoQuery.OrderBy(t => t.Name),
            "name_desc" => todoQuery.OrderByDescending(t => t.Name),
            "created" => todoQuery.OrderBy(t => t.CreatedOnUtc),
            "created_desc" => todoQuery.OrderByDescending(t => t.CreatedOnUtc),
            _ => todoQuery.OrderByDescending(t => t.CreatedOnUtc) // Default
        };
        
        // Project to DTO
        var todos = await todoQuery
            .Select(t => new TodoResponse(
                t.Id,
                t.Name,
                t.Description,
                t.Status,
                t.Priority,
                t.DueDate,
                t.IsCompleted,
                t.CompletedAt,
                t.CreatedOnUtc,
                t.CreatedByUserName ?? "Unknown"))
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);
        
        // Get total count for pagination
        var totalCount = await context.Todos
            .Where(t => t.TenantId == currentUser.GetTenant())
            .CountAsync(cancellationToken);
        
        return new PagedList<TodoResponse>(todos, totalCount, query.PageNumber, query.PageSize);
    }
}
```

---

### 12. Module Registration (IModule)

**File**: `Modules.{Module}/{Module}Module.cs`

```csharp
using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Modules.Todos.Data;
using FSH.Modules.Todos.Features.v1.Todos.ArchiveTodo;
using FSH.Modules.Todos.Features.v1.Todos.CreateTodo;
using FSH.Modules.Todos.Features.v1.Todos.DeleteTodo;
// ... more imports
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.Todos;

/// <summary>
/// Todo Module - A comprehensive module for managing todo items and tasks.
/// 
/// **Purpose:**
/// Provides a complete feature set for creating, managing, and tracking todo items with support for:
/// - Master-Detail relationship between Todos and TodoTasks
/// - Status tracking and priority management
/// - Completion tracking with timestamps
/// - Archiving functionality for inactive todos
/// - Bulk import/export operations
/// - Full audit trail for compliance and tracking
/// 
/// **Key Features:**
/// - CRUD operations for todos
/// - Task management within todos
/// - Status transitions and completion tracking
/// - Priority-based organization
/// - Archive/Restore functionality
/// - Bulk import/export
/// 
/// **Architecture:**
/// - Entity Framework Core with PostgreSQL/MSSQL support
/// - Domain-Driven Design with aggregate pattern
/// - Multi-tenancy support
/// - CQRS pattern with Mediator library
/// - FluentValidation for request validation
/// 
/// **Entities:**
/// - Todo: Master aggregate
/// - TodoTask: Detail entity
/// 
/// **Permissions:**
/// See TodoPermissionConstants class
/// </summary>
public class TodoModule : IModule
{
    /// <summary>
    /// Configures dependency injection and services for the Todo module.
    /// 
    /// Called during application startup to register:
    /// - Permissions
    /// - DbContext
    /// - Database initializer
    /// - Health checks
    /// </summary>
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(TodoPermissionConstants.GetPermissions());
        
        // Register DbContext with multi-tenancy support
        builder.Services.AddHeroDbContext<TodoDbContext>();
        
        // Register database initializer
        builder.Services.AddScoped<IDbInitializer, TodoDbInitializer>();
        
        // Register health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<TodoDbContext>(
                name: "db:todo",
                failureStatus: HealthStatus.Unhealthy);
    }
    
    /// <summary>
    /// Maps all endpoints for the Todo module.
    /// 
    /// Called after ConfigureServices to register HTTP endpoints with:
    /// - API versioning (v1)
    /// - Route grouping (/api/v{version}/todo)
    /// - Endpoint tags for OpenAPI documentation
    /// </summary>
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // API versioning setup
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        // Route group configuration
        RouteGroupBuilder group = endpoints
            .MapGroup("api/v{version:apiVersion}/todo")
            .WithTags("Todos")
            .WithApiVersionSet(apiVersionSet);
        
        // Map all Todo endpoints
        group.MapGetTodosEndpoint();
        group.MapGetTodoEndpoint();
        group.MapCreateTodoEndpoint();
        group.MapUpdateTodoEndpoint();
        group.MapDeleteTodoEndpoint();
        group.MapCompleteTodoEndpoint();
        group.MapReopenTodoEndpoint();
        group.MapUpdateTodoStatusEndpoint();
        group.MapArchiveTodoEndpoint();
        group.MapExportTodosEndpoint();
        group.MapImportTodosEndpoint();
        
        // Map all TodoTask endpoints
        group.MapGetTodoTasksEndpoint();
        group.MapCreateTodoTaskEndpoint();
        group.MapUpdateTodoTaskEndpoint();
        group.MapDeleteTodoTaskEndpoint();
        group.MapToggleTaskCompletionEndpoint();
        group.MapReorderTasksEndpoint();
    }
}
```

---

### 13. Global Usings File

**File**: `Modules.{Module}/GlobalUsings.cs`

```csharp
// Framework
global using FSH.Framework.Core.Context;
global using FSH.Framework.Core.Domain;
global using FSH.Framework.Persistence;
global using FSH.Framework.Shared.Multitenancy;
global using FSH.Modules.Todos.Domain;
global using FSH.Modules.Todos.Exceptions;

// Mediator
global using Mediator;

// Microsoft
global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;

// Utilities
global using System.Collections.Generic;
global using System.Linq;
global using System.Threading;
global using System.Threading.Tasks;
```

---

## 🎯 Feature Implementation Checklist

When implementing a new feature in a module, follow this checklist:

### 1. Contract Definition (Contracts Project)
- [ ] Create command/query record in `Contracts/v1/{Feature}/`
- [ ] Create response record (if applicable)
- [ ] Use meaningful names: `{Feature}Command`, `{Feature}Query`, `{Feature}Response`
- [ ] Include XML comments explaining purpose and parameters
- [ ] Set appropriate response type (`ICommand<TResponse>`, `IQuery<TResponse>`)

### 2. Implementation (Module Project)
- [ ] Create validator using FluentValidation
- [ ] Use validation extension methods for DRY code
- [ ] Reference string length constants
- [ ] Create handler implementing `ICommandHandler<,>` or `IQueryHandler<,>`
- [ ] Use async/await with CancellationToken
- [ ] Handle exceptions appropriately (throw centralized exceptions)
- [ ] Create endpoint as static extension method
- [ ] Use appropriate HTTP method (GET for queries, POST for commands)
- [ ] Add permission requirements
- [ ] Include OpenAPI documentation

### 3. Domain Model
- [ ] Use factory methods for entity creation
- [ ] Implement domain methods for state transitions
- [ ] Validate in factory methods and domain methods
- [ ] Track audit information (CreatedBy, ModifiedBy, timestamps)
- [ ] Support multi-tenancy (TenantId)

### 4. Database
- [ ] Create entity configuration (IEntityTypeConfiguration)
- [ ] Configure table schema and properties
- [ ] Define constraints and relationships
- [ ] Create indexes for performance
- [ ] Use string length constants in HasMaxLength()

### 5. Error Handling
- [ ] Create specific exception classes inheriting from framework exceptions
- [ ] Use extension methods to throw exceptions consistently
- [ ] Avoid duplicate exception throws across handlers

### 6. Testing
- [ ] Happy path scenario (data is valid, operation succeeds)
- [ ] Validation errors (invalid data, returns 400)
- [ ] Not found errors (resource doesn't exist, returns 404)
- [ ] Permission errors (user lacks permission, returns 403)
- [ ] Multi-tenancy isolation (user only sees own tenant data)

---

## 📋 Common Patterns

### Pattern 1: Create with Validation

```csharp
// Handler
var entity = DomainEntity.Create(
    command.Name,
    currentUser.GetTenant(),
    currentUser.GetUserId(),
    currentUser.Name);

context.Entities.Add(entity);
await context.SaveChangesAsync(ct);
return entity.Id;

// Domain
public static DomainEntity Create(string name, string tenantId, Guid userId, string userName)
{
    ArgumentException.ThrowIfNullOrWhiteSpace(name);
    ArgumentException.ThrowIfNullOrWhiteSpace(tenantId);
    
    return new DomainEntity
    {
        Id = Guid.NewGuid(),
        Name = name,
        TenantId = tenantId,
        CreatedBy = userId,
        CreatedByUserName = userName,
        CreatedOnUtc = DateTimeOffset.UtcNow
    };
}
```

### Pattern 2: State Transition with Domain Event

```csharp
// Handler
var todo = await context.Todos.FindAsync(new object[] { command.Id }, cancellationToken: ct)
    ?? throw new TodoNotFoundException(command.Id);

todo.Complete(); // Domain method

await context.SaveChangesAsync(ct);
return Unit.Value;

// Domain
public void Complete()
{
    IsCompleted = true;
    CompletedAt = DateTimeOffset.UtcNow;
    Status = nameof(TodoStatus.Completed);
    LastModifiedOnUtc = DateTimeOffset.UtcNow;
    
    // Could raise domain event here
    // RaiseDomainEvent(new TodoCompletedEvent(Id));
}
```

### Pattern 3: List with Pagination and Filtering

```csharp
// Handler
var query = context.Entities
    .AsNoTracking()
    .Where(e => e.TenantId == currentUser.GetTenant());

// Apply filters
if (!string.IsNullOrWhiteSpace(search))
    query = query.Where(e => e.Name.Contains(search));

// Apply sorting and pagination
var items = await query
    .OrderByDescending(e => e.CreatedOnUtc)
    .Skip((pageNumber - 1) * pageSize)
    .Take(pageSize)
    .Select(e => new EntityResponse(e.Id, e.Name, ...))
    .ToListAsync(ct);

var totalCount = await context.Entities
    .Where(e => e.TenantId == currentUser.GetTenant())
    .CountAsync(ct);

return new PagedList<EntityResponse>(items, totalCount, pageNumber, pageSize);
```

### Pattern 4: Exception Throwing with Extensions

```csharp
// Exception class
public static class TodoExceptionExtensions
{
    public static Todo ThrowIfNotFound(this Todo? todo, Guid id)
        => todo ?? throw new TodoNotFoundException(id);
}

// Handler
var todo = await context.Todos.FindAsync(new object[] { id }, cancellationToken: ct);
todo.ThrowIfNotFound(id);
```

---

## ✅ Best Practices Summary

1. **String Lengths**: Always use power-of-2 constants (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048)
2. **Exceptions**: Centralize exception creation in dedicated classes with extension methods
3. **Domain Logic**: Use factory methods and domain methods for business logic (not handlers)
4. **Validation**: Use extension methods for DRY validation rules
5. **Queries**: Always use `.AsNoTracking()` for read-only operations
6. **Pagination**: Apply at database level using `.Skip()` and `.Take()`
7. **Multi-Tenancy**: Always filter by `currentUser.GetTenant()` or `currentUser.TenantId`
8. **Async**: Always use `async/await` with `CancellationToken`
9. **Documentation**: Add XML comments to public types and methods
10. **Permissions**: Always use `.RequirePermission()` on endpoints

---

## 🧪 Testing Examples

### Command Handler Test

```csharp
[Fact]
public async Task CreateTodoCommand_WithValidData_ReturnsTodoId()
{
    // Arrange
    var currentUser = new MockCurrentUser { UserId = Guid.NewGuid(), TenantId = "test" };
    var context = new TodoDbContextMock();
    var handler = new CreateTodoCommandHandler(context, currentUser);
    
    var command = new CreateTodoCommand("Test Todo", null, null, 1, null);
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    result.Should().NotBeEmpty();
    context.Todos.Should().HaveCount(1);
}

[Fact]
public async Task CreateTodoCommand_WithEmptyName_ThrowsValidationException()
{
    // Arrange
    var validator = new CreateTodoCommandValidator();
    var command = new CreateTodoCommand("", null, null, 1, null);
    
    // Act
    var result = await validator.ValidateAsync(command);
    
    // Assert
    result.IsValid.Should().BeFalse();
    result.Errors.Should().Contain(e => e.PropertyName == "Name");
}
```

---

## 📚 Additional Resources

- `COPILOT_INSTRUCTIONS.md` - General architecture patterns
- `ARCHITECTURE_GUIDE.md` - Deep dive into architecture decisions
- `MODULE_TEMPLATES.md` - Module type templates and decision trees
- Todo Module source - Reference implementation
- Architecture Tests - Rules enforcement

---

## 🔄 Workflow Summary

1. **Study**: Read this document and review existing modules
2. **Plan**: Decide feature scope using MODULE_TEMPLATES.md
3. **Implement**: Follow patterns from this document
4. **Test**: Write tests covering happy path and edge cases
5. **Verify**: Run architecture tests and build solution
6. **Document**: Add comprehensive XML comments
7. **Review**: Check against this checklist before submitting

---

**Last Updated**: 2026-01-04
**Based on**: Todo Module v1.0 Reference Implementation
