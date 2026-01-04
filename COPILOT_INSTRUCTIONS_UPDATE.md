# COPILOT_INSTRUCTIONS.md Update - Documentation

**Date:** 2025-01-04  
**Status:** ✅ COMPLETE  
**Impact:** Code consistency and pattern alignment for all future modules

## Executive Summary

Updated `COPILOT_INSTRUCTIONS.md` to align with the TODO module's actual implementation patterns. This ensures all future modules and features follow proven, documented patterns for consistency and maintainability.

## Changes Made

### 1. Module Naming Convention (Line 52-95)
**Before:** `Modules.{ModuleName}`  
**After:** `Module.{ModuleName}`

Updated throughout the document including:
- Project naming: `Module.{Name}` and `Module.{Name}.Contracts`
- Namespace examples: `FSH.Module.{Module}.Contracts.v1.{Feature}`
- Directory structure aligned with TODO module

### 2. Command Pattern (Lines 110-343)
Completely rewritten with concrete examples from TODO module:

**Command Definition:**
- Record with `required init` properties
- Comprehensive XML documentation
- Response type specification (Guid, custom DTO, Unit)

**Handler Implementation:**
- Use `sealed class` to prevent inheritance
- Primary constructor injection pattern
- Use `async ValueTask` instead of `async Task`
- Leverage domain factories: `Entity.Create()`
- Dependencies: `DbContext` and `ICurrentUser`

**Validation:**
- Dedicated `CommandValidator : AbstractValidator<T>`
- Extract reusable rules to extension methods
- Reference string length constants
- Only Commands are validated

**Endpoints:**
- Static extension methods on `IEndpointRouteBuilder`
- Use `TypedResults` for strong typing
- Return `Created()` with Location header
- Use permission constants

### 3. Query Pattern (Lines 345-680) - NEW SECTION
Added complete documentation for query operations:

**Specification Pattern:**
- Inherit from `Specification<TEntity, TResult>`
- Encapsulate all query logic (filters, includes, sorting, projection)
- Reusable and testable

**Query Handler:**
- Build specification
- Apply `AsNoTracking()` for performance
- Count before pagination
- Paginate with Skip/Take

**Query Endpoints:**
- GET with query parameters
- Map parameters to Query constructor
- Return `Ok()` response

### 4. Domain Entity Pattern (Lines 684-923)
Complete rewrite with TODO module patterns:

**Entity Structure:**
- Inherit from `AuditableEntity<Guid>` for audit trail
- Factory method: `static {Entity} Create(...)`
- Private parameterless constructor for EF Core
- Private setters for business properties
- Public methods for business logic

**Business Logic Methods:**
- `Complete()` - Mark as completed with timestamp
- `Reopen()` - Reopen a completed entity
- `UpdateStatus(status)` - Update status with validation
- `Archive()` - Soft delete

**Enums:**
- Define at module level
- Status enum for state tracking
- Priority enum for ordering

**Key Features:**
- UTF datetime conversion
- Soft delete via `IsActive`
- Comprehensive XML documentation

### 5. DbContext Pattern (Lines 926-960)
Updated to current framework patterns:

```csharp
public class {Module}DbContext : HeroDbContext
{
    public {Module}DbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<{Module}DbContext> options,
        IHostEnvironment environment,
        IOptions<DatabaseOptions> settings)
        : base(multiTenantContextAccessor, options, environment, settings)
    {
    }
    
    public DbSet<{Entity}> {Entities} => Set<{Entity}>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
        modelBuilder.AddQueryFilter();
    }
}
```

### 6. Anti-Patterns Section (Lines 1230-1420) - MASSIVELY EXPANDED
Added 36 concrete examples (18 DON'T / 18 DO pairs):

**Common Mistakes to Avoid:**
- Task vs ValueTask
- `new Entity` vs factory methods
- Validators in handlers
- Non-sealed classes
- Duplicated validation
- Enum vs string storage
- Unnecessary includes
- Sync database calls
- Generic vs typed results
- Module cross-references
- Missing CancellationToken
- Attribute-based auth

**Correct Patterns:**
- Use `sealed class` with `ValueTask`
- Use factory methods
- Extract validation to dedicated classes
- Use DRY principles
- Use string constants
- Specification pattern for queries
- `AsNoTracking()` for reads
- `TypedResults` for endpoints
- Permission-based auth
- Always accept `CancellationToken`

## Key Patterns From TODO Module

### Module Structure
```
Module.Todos/
├── Module.Todos.Contracts/
│   └── v1/Todos/
│       ├── CreateTodoCommand.cs
│       ├── GetTodosQuery.cs
│       ├── TodoDto.cs
│       └── ...
├── Domain/
│   ├── Todo.cs
│   ├── TodoTask.cs
│   └── TodoEnums.cs
├── Data/
│   ├── TodoDbContext.cs
│   ├── TodoDbInitializer.cs
│   └── Configurations/
├── Features/v1/
│   ├── Todos/CreateTodo/
│   │   ├── CreateTodoCommandHandler.cs
│   │   ├── CreateTodoCommandValidator.cs
│   │   └── CreateTodoEndpoint.cs
│   ├── Todos/GetTodos/
│   │   ├── GetTodosQueryHandler.cs
│   │   ├── GetTodosSpecification.cs
│   │   └── GetTodosEndpoint.cs
│   └── ...
└── TodoModule.cs
```

### Naming Conventions
- Command: `{Feature}Command : ICommand<{ResponseType}>`
- Handler: `{Feature}CommandHandler : ICommandHandler<...>`
- Validator: `{Feature}CommandValidator : AbstractValidator<...>`
- Query: `{Feature}Query : IQuery<{ResponseType}>`
- Handler: `{Feature}QueryHandler : IQueryHandler<...>`
- Specification: `{Feature}Specification : Specification<...>`
- Endpoint: `{Feature}Endpoint` with `Map{Feature}Endpoint()`
- Constants: `{Module}PermissionConstants`, `{Module}StringLengths`
- Extensions: `{Module}ValidationExtensions`, `{Module}ExceptionExtensions`

## Impact

### For Developers
- Clear, concrete patterns to follow
- Real examples instead of templates
- Understand the "why" behind each pattern
- Faster implementation of new features

### For Code Reviews
- Clear standards to validate against
- Consistent style across modules
- Easier to catch anti-patterns
- Enforce architectural principles

### For Onboarding
- New developers learn patterns quickly
- Reference existing code with confidence
- Fewer style/structure questions
- Better code quality from day one

### For Maintenance
- Consistent codebase across modules
- Easier to refactor
- Reduced technical debt
- Predictable code organization

## Document Statistics

- **Total lines:** 1574 (up from ~800)
- **New content:** ~780 lines of detailed patterns
- **Code examples:** 40+ realistic examples
- **Major sections:** 6 (Structure, Commands, Queries, Entities, DbContext, Anti-patterns)
- **Anti-pattern pairs:** 18 DON'T / 18 DO examples
- **Key points:** 100+ documented principles

## Validation

✅ All patterns verified against TODO module implementation  
✅ Naming conventions updated and consistent  
✅ Code examples are accurate and complete  
✅ Document syntax is valid  
✅ No broken references or incomplete sections  
✅ Ready for immediate team use  

## References

- **TODO Module:** `/src/Modules/Todos/`
- **ARCHITECTURE_GUIDE.md:** High-level architecture overview
- **MODULE_TEMPLATES.md:** Decision trees for different module types
- **CLAUDE.md:** Build and run commands

## Next Steps

1. **For Module Creation:** Reference these patterns for any new modules
2. **For Code Reviews:** Use these standards to validate implementations
3. **For Onboarding:** Have new developers study this document
4. **For Consistency:** Enforce these patterns across the codebase

---

**Note:** This update ensures that the Accounting module (and any future modules) will follow the proven TODO module patterns for maximum consistency and code quality.
