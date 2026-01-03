# Todo Module - Best Practices Review (DRY & SOLID)

## Executive Summary

**Overall Score: 7.0/10**

The Todo module demonstrates solid architectural principles with good SOLID compliance, but has several DRY violations and consistency issues that should be addressed.

---

## 1. DRY PRINCIPLE - VIOLATIONS FOUND

### 🔴 CRITICAL: Reflection Misuse in ReorderTasksCommandHandler

**Location:** `Features/v1/TodoTasks/ReorderTasks/ReorderTasksCommandHandler.cs` (Lines 23-25)

```csharp
typeof(Domain.TodoTask)
    .GetProperty(nameof(Domain.TodoTask.SortOrder))!
    .SetValue(task, orderItem.SortOrder);
```

**Problems:**
- Uses reflection instead of domain method
- Bypasses validation logic
- Doesn't update `LastModifiedOnUtc` (breaks audit trail)
- Performance overhead
- Difficult to test and maintain

**Solution:**
Add a domain method to `TodoTask`:

```csharp
public void UpdateSortOrder(int newSortOrder)
{
    SortOrder = newSortOrder;
    LastModifiedOnUtc = DateTimeOffset.UtcNow;
}

// In handler:
foreach (var orderItem in command.Tasks)
{
    var task = tasks.FirstOrDefault(t => t.Id == orderItem.TaskId);
    if (task != null)
    {
        task.UpdateSortOrder(orderItem.SortOrder);
    }
}
```

**Priority:** 🔴 **CRITICAL**

---

### ⚠️ HIGH: Exception Handling Inconsistency

**Problem:** Different handlers use different exception types for the same scenario.

**Examples:**

❌ `CompleteTodoCommandHandler.cs` (Lines 16-19):
```csharp
if (todo == null)
{
    throw new InvalidOperationException($"Todo with ID {command.Id} not found");
}
```

❌ `ReopenTodoCommandHandler.cs` (Lines 13-15):
```csharp
var todo = await dbContext.Todos
    .FirstOrDefaultAsync(t => t.Id == command.Id, cancellationToken)
    ?? throw new NotFoundException($"Todo with id {command.Id} not found.");
```

❌ `UpdateTodoCommandHandler.cs`:
- Uses `InvalidOperationException`

❌ `DeleteTodoCommandHandler.cs`:
- Uses `InvalidOperationException`

**Impact:**
- Inconsistent error handling at application level
- Different HTTP status codes returned
- Difficult to write consistent error handling
- Reduces code maintainability

**Solution:**
Standardize on `NotFoundException` for all 404 scenarios. Create extension method:

```csharp
public static class TodoQueryExtensions
{
    public static async Task<T> GetByIdOrThrowAsync<T>(
        this IQueryable<T> query,
        Guid id,
        string entityName,
        CancellationToken ct) where T : class
    {
        return await query.FirstOrDefaultAsync(ct)
            ?? throw new NotFoundException($"{entityName} with id {id} not found.");
    }
}

// Usage:
var todo = await context.Todos.Where(t => t.Id == command.Id)
    .GetByIdOrThrowAsync("Todo", command.Id, ct);
```

**Priority:** 🔴 **CRITICAL**

---

### ⚠️ MEDIUM: Handler Pattern Repetition

**Affected Handlers:**
- `CompleteTodoCommandHandler`
- `ReopenTodoCommandHandler`
- `ArchiveTodoCommandHandler`
- Similar patterns in TodoTask handlers

**Repetitive Pattern:**

```csharp
// Pattern found across multiple handlers:
1. Get entity from database
2. Check if null → throw exception
3. Call domain method (Complete, Reopen, Archive)
4. SaveChangesAsync
5. Return Unit.Value
```

**Example Duplication:**

```csharp
// CompleteTodo
var todo = await context.Todos.FirstOrDefaultAsync(...);
if (todo == null) throw new...;
todo.Complete();
await context.SaveChangesAsync();

// Reopen
var todo = await dbContext.Todos.FirstOrDefaultAsync(...) ?? throw new...;
todo.Reopen();
await dbContext.SaveChangesAsync();

// Archive
var todo = await dbContext.Todos.FirstOrDefaultAsync(...) ?? throw new...;
todo.Archive();
await dbContext.SaveChangesAsync();
```

**Solution:**
Create generic extension methods or base class to extract the pattern.

**Priority:** 🟠 **HIGH**

---

### ⚠️ MEDIUM: Validator Rules Duplication

**Location:** 
- `CreateTodo/CreateTodoCommandValidator.cs`
- `UpdateTodo/UpdateTodoCommandValidator.cs`

**Problem:** Same validation rules appear in both validators.

```csharp
// Both have identical:
RuleFor(x => x.Name)
    .NotEmpty().WithMessage("Name is required")
    .MaximumLength(200).WithMessage("Name must not exceed 200 characters");

RuleFor(x => x.Description)
    .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

RuleFor(x => x.Notes)
    .MaximumLength(2000).WithMessage("Notes must not exceed 2000 characters");

RuleFor(x => x.Priority)
    .InclusiveBetween(1, 4).WithMessage("Priority must be between 1 and 4");
```

**Solution:**
Create shared validation rules helper:

```csharp
public static class TodoValidationRules
{
    public const int NameMaxLength = 200;
    public const int DescriptionMaxLength = 2000;
    public const int NotesMaxLength = 2000;
    public const int MinPriority = 1;
    public const int MaxPriority = 4;
    
    public static IRuleBuilderOptions<T, string> ValidateTodoName<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(NameMaxLength)
            .WithMessage($"Name must not exceed {NameMaxLength} characters");
    }
    
    public static IRuleBuilderOptions<T, string> ValidateTodoDescription<T>(
        this IRuleBuilder<T, string> ruleBuilder)
    {
        return ruleBuilder
            .MaximumLength(DescriptionMaxLength)
            .WithMessage($"Description must not exceed {DescriptionMaxLength} characters");
    }
    
    public static IRuleBuilderOptions<T, int> ValidateTodoPriority<T>(
        this IRuleBuilder<T, int> ruleBuilder)
    {
        return ruleBuilder
            .InclusiveBetween(MinPriority, MaxPriority)
            .WithMessage($"Priority must be between {MinPriority} and {MaxPriority}");
    }
}

// Usage in validators:
public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
{
    public CreateTodoCommandValidator()
    {
        RuleFor(x => x.Name).ValidateTodoName();
        RuleFor(x => x.Description).ValidateTodoDescription();
        RuleFor(x => x.Notes).ValidateTodoDescription();
        RuleFor(x => x.Priority).ValidateTodoPriority();
    }
}
```

**Priority:** 🟠 **HIGH**

---

## 2. SOLID PRINCIPLES ASSESSMENT

### ✅ Single Responsibility Principle: 8/10

**Strengths:**
- Each handler focuses on one command/query
- Validators focus only on validation
- Endpoints map HTTP to commands/queries
- DbContext manages data access
- Domain entities encapsulate business logic

**Minor Issues:**
- `ImportTodosCommandHandler` does multiple things:
  - Selects importer based on content type
  - Creates domain objects
  - Saves to database

**Recommendation:** Extract import logic into `ITodoImportService`

---

### ⚠️ Open/Closed Principle: 6.5/10

**Issue 1: Hard-coded Importer Selection**

```csharp
// ImportTodosCommandHandler.cs (Lines 16-26)
if (command.ContentType.Contains("json", StringComparison.OrdinalIgnoreCase))
{
    importer = new JsonDataImporter<TodoImportDto>();
}
else if (command.ContentType.Contains("csv", StringComparison.OrdinalIgnoreCase))
{
    importer = new CsvDataImporter<TodoImportDto>();
}
```

**Problem:** Adding new formats requires modifying handler (violates OCP)

**Solution:** Use factory pattern with dependency injection

```csharp
public interface IImporterFactory
{
    IDataImporter<T> GetImporter<T>(string contentType);
}

// Usage in handler:
var importer = _importerFactory.GetImporter<TodoImportDto>(command.ContentType);
```

**Priority:** 🟠 **HIGH**

---

### ✅ Liskov Substitution Principle: 8/10

**Strengths:**
- Handlers properly implement `ICommandHandler`/`IQueryHandler`
- Domain entities properly inherit from `AuditableEntity`
- Validators inherit from `AbstractValidator`
- No suspicious downcasting

**Assessment:** No violations found.

---

### ✅ Interface Segregation Principle: 8/10

**Strengths:**
- `ICommandHandler<TCommand, TResult>` (with return value)
- `ICommandHandler<TCommand>` (void/Unit return)
- `IQueryHandler<TQuery, TResult>` (read-only)
- `IModule` is focused
- Small, specific interfaces

**Assessment:** Following best practices well.

---

### ✅ Dependency Inversion Principle: 8/10

**Strengths:**
- Handlers depend on abstractions (`TodoDbContext`)
- Uses `IMediator` for command dispatch
- `ICurrentUser` abstraction for context
- Permission checks via abstraction

**Minor Issue:**
- `ReorderTasksCommandHandler` uses reflection instead of domain method (see Critical section)

---

## 3. ADDITIONAL ISSUES

### ❌ CRITICAL: Missing Foreign Key Validation

**Location:** `CreateTodoTask/CreateTodoTaskCommandHandler.cs`

**Problem:** No validation that parent Todo exists before creating task.

```csharp
// Current code - no validation!
public async ValueTask<Guid> Handle(CreateTodoTaskCommand command, ...)
{
    var task = TodoTask.Create(command.TodoId, ...);  // ❌ No validation!
    context.TodoTasks.Add(task);
    await context.SaveChangesAsync(cancellationToken);
    return task.Id;
}
```

**Impact:** Database constraint will fail at save time, causing poor UX

**Solution:**

```csharp
public async ValueTask<Guid> Handle(CreateTodoTaskCommand command, ...)
{
    var todoExists = await context.Todos
        .AnyAsync(t => t.Id == command.TodoId, cancellationToken);
    
    if (!todoExists)
        throw new NotFoundException($"Todo with id {command.TodoId} not found.");
    
    var task = TodoTask.Create(command.TodoId, ...);
    context.TodoTasks.Add(task);
    await context.SaveChangesAsync(cancellationToken);
    return task.Id;
}
```

**Priority:** 🔴 **CRITICAL**

---

### ❌ CRITICAL: Missing Permission Check

**Location:** `CompleteTodo/CompleteTodoEndpoint.cs` (Line 27)

**Problem:**
```csharp
.RequireAuthorization();  // ❌ No specific permission!
```

**Other endpoints correctly use:**
```csharp
.RequirePermission(TodoPermissionConstants.Todos.Update);  // ✅ Correct
```

**Fix:**
```csharp
.RequirePermission(TodoPermissionConstants.Todos.Update);
```

**Also check:** `ReopenTodoEndpoint` and other status-change endpoints

**Priority:** 🔴 **CRITICAL**

---

### ⚠️ MEDIUM: Naming Inconsistency

**Problem 1: Parameter Names**

```csharp
// Inconsistent parameter naming:
public sealed class CreateTodoCommandHandler(TodoDbContext context)  // ✓ context
public sealed class CompleteTodoCommandHandler(TodoDbContext context)  // ✓ context
public class ReopenTodoCommandHandler(TodoDbContext dbContext)  // ✗ dbContext
public class ArchiveTodoCommandHandler(TodoDbContext dbContext)  // ✗ dbContext
```

**Problem 2: Sealed Modifier**

```csharp
public sealed class CreateTodoCommandHandler(...)  // ✓ sealed
public class ReopenTodoCommandHandler(...)  // ✗ no sealed
public class ArchiveTodoCommandHandler(...)  // ✗ no sealed
```

**Fix:**
- Standardize to `context` (not `dbContext`)
- Add `sealed` to all handler classes

**Priority:** 🟡 **MEDIUM**

---

## 4. STRENGTHS

✅ **Good Architecture Foundation**
- CQRS pattern well implemented
- DDD principles followed
- Master-Detail relationship properly modeled
- Clean separation of concerns

✅ **Good Async/Await Usage**
- Proper async/await throughout
- `CancellationToken` properly threaded
- `ValueTask` used for handlers (performance optimization)

✅ **Good Error Handling (mostly)**
- Exception handling present
- Proper null checks
- Domain validation in place

✅ **Good Permission System**
- Granular permissions defined
- Most endpoints properly protected
- Clear permission naming

✅ **Good Data Layer**
- Multi-tenancy support
- Proper EF Core configuration
- Database indexes for performance
- Seed data for testing

---

## 5. ACTION ITEMS BY PRIORITY

### 🔴 CRITICAL (Fix Immediately)

1. **Remove Reflection** in `ReorderTasksCommandHandler`
   - Add `UpdateSortOrder()` method to `TodoTask` domain
   - Update handler to use domain method
   - Ensure audit trail is updated

2. **Standardize Exception Types**
   - Replace `InvalidOperationException` with `NotFoundException`
   - Use consistent error messages format
   - Create extension method for `GetByIdOrThrowAsync`

3. **Add Missing Permission Checks**
   - `CompleteTodoEndpoint` → Add `UpdateTodo` permission
   - `ReopenTodoEndpoint` → Add `UpdateTodo` permission

4. **Add Foreign Key Validation**
   - `CreateTodoTaskCommandHandler` must validate Todo exists
   - Consider adding validator check as well

### 🟠 HIGH (Next Sprint)

1. **Extract `TodoValidationRules` Helper Class**
   - Create shared validation rules for Name, Description, Notes, Priority
   - Use extension methods in validators
   - Significant DRY improvement

2. **Create `TodoQueryExtensions`**
   - Add `GetByIdOrThrowAsync` extension
   - Add `FindByIdAsync` extension
   - Reduce duplication across handlers

3. **Standardize Naming**
   - Use `context` consistently (not `dbContext`)
   - Add `sealed` to all handlers
   - Rename parameters consistently

### 🟡 MEDIUM (Future Enhancement)

1. **Create `ImporterFactory`**
   - Implement factory pattern for importer selection
   - Improve OCP compliance
   - Easy to add new formats

2. **Extract Handler Base Class** (Optional)
   - Extract common get-update-save pattern
   - Reduce handler code duplication
   - Consistent error handling

3. **Create `ITodoImportService`**
   - Separate concerns in `ImportTodosCommandHandler`
   - Dedicated service for import operations

---

## 6. SUMMARY SCORECARD

| Principle | Score | Status | Comment |
|-----------|-------|--------|---------|
| **DRY** | 6/10 | ⚠️ | Exception handling, handlers, validators repetitive |
| **SRP** | 8/10 | ✅ | Clear separation of concerns |
| **OCP** | 6.5/10 | ⚠️ | Hard-coded importer selection |
| **LSP** | 8/10 | ✅ | Proper interface implementations |
| **ISP** | 8/10 | ✅ | Focused, segregated interfaces |
| **DIP** | 8/10 | ✅ | Good dependency injection |
| **Async/Await** | 9/10 | ✅ | Excellent async pattern usage |
| **Error Handling** | 5/10 | ❌ | Inconsistent exceptions, missing validation |
| **Naming** | 7/10 | ⚠️ | Some inconsistencies |
| **Permissions** | 6/10 | ⚠️ | Missing checks |
| **Overall** | **7.0/10** | ⚠️ | **Good with improvements needed** |

---

## 7. CONCLUSION

The Todo module demonstrates solid architectural design with good SOLID compliance. However, several DRY violations and consistency issues should be addressed:

**✓ Good:**
- CQRS and DDD architecture sound
- Separation of concerns well done
- Async/await properly implemented
- Multi-tenancy support solid

**✗ Issues:**
- Reflection misuse is critical
- Exception handling inconsistent
- Validation rules duplicated
- Missing foreign key validation
- Missing one permission check

**Estimated Effort:** 1-2 sprints to fix all critical/high items

**Recommended Approach:**
1. Fix critical issues first
2. Extract common patterns
3. Refactor handlers to use helpers
4. Add factory pattern for imports
5. Add comprehensive tests

