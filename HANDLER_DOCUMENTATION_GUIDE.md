# Phase 2: Documentation Standards - Handler XML Documentation Guide

## Overview
This guide provides standardized XML documentation patterns for all handlers, queries, and commands in the Accounting module. Following these patterns ensures consistency and helps developers understand the purpose, behavior, and usage of each handler.

## Handler Documentation Pattern

### 1. Command/Query Records
Every command and query record should include:

```csharp
/// <summary>
/// [Operation] [Entity] command.
/// 
/// **Purpose:**
/// [Clear explanation of what this command does]
/// 
/// **Parameters:**
/// - [ParamName]: [Description and constraints]
/// 
/// **Multi-Tenancy:**
/// [How tenant context is handled - usually automatic from ICurrentUser]
/// 
/// **Validation:**
/// Validated by [ValidatorClassName] to ensure:
/// - [Validation rule 1]
/// - [Validation rule 2]
/// 
/// **Side Effects:**
/// - [Any domain events, cascading effects, or state changes]
/// </summary>
public record [CommandName]([Parameters]) : ICommand[<ReturnType>];
```

### 2. Handler Classes
Every handler class should include:

```csharp
/// <summary>
/// Handler for [Command/Query Name].
/// 
/// **Responsibility:**
/// [What the handler does - usually CRUD or business logic operation]
/// 
/// **Execution Flow:**
/// 1. [First step]
/// 2. [Second step]
/// 3. [Save/Return result]
/// 
/// **Permissions:**
/// Requires: [Permission constant needed]
/// 
/// **Return Value:**
/// [What is returned on success]
/// 
/// **Exceptions:**
/// - [ExceptionType]: [When thrown]
/// </summary>
public class [HandlerName] : ICommandHandler<[CommandName], [ReturnType]>
```

### 3. Handler Methods
The `Handle` method should include parameter documentation:

```csharp
/// <summary>
/// Handles the [Operation] request.
/// </summary>
/// <param name="command">The [Operation] command with [key properties]</param>
/// <param name="ct">Cancellation token for the operation</param>
/// <returns>[Description of return value]</returns>
/// <exception cref="NotFoundException">Thrown when [entity] is not found</exception>
/// <exception cref="BadRequestException">Thrown when [validation fails]</exception>
public async ValueTask<[ReturnType]> Handle([CommandName] command, CancellationToken ct)
```

## Common Handler Patterns

### Create Handler Pattern
```csharp
/// <summary>
/// Handler for creating a new [Entity] entity.
/// 
/// **Responsibility:**
/// Creates a new [Entity] with the provided properties and persists it to the database.
/// 
/// **Execution Flow:**
/// 1. Validate command parameters
/// 2. Call [Entity].Create() factory method
/// 3. Add to DbSet and save
/// 4. Return new entity ID
/// 
/// **Permissions:**
/// Requires: [Module].[Entity].Create
/// </summary>
public class Create[Entity]Handler : ICommandHandler<Create[Entity]Command, Guid>
```

### Get Handler Pattern
```csharp
/// <summary>
/// Handler for retrieving a single [Entity] by ID.
/// 
/// **Responsibility:**
/// Retrieves a [Entity] from the database and returns it as a DTO.
/// 
/// **Execution Flow:**
/// 1. Query database for [Entity] by ID
/// 2. Throw NotFoundException if not found
/// 3. Map to DTO
/// 4. Return result
/// 
/// **Permissions:**
/// Requires: [Module].[Entity].View
/// </summary>
public class Get[Entity]Handler : IQueryHandler<Get[Entity]Query, [Entity]Dto>
```

### GetList Handler Pattern
```csharp
/// <summary>
/// Handler for retrieving paginated list of [Entity] entities.
/// 
/// **Responsibility:**
/// Retrieves a paginated, filtered, and sorted list of [Entity] entities.
/// 
/// **Filtering:**
/// - Supports filtering by: [property], [property]
/// - Supports text search in: [property]
/// 
/// **Sorting:**
/// Default: CreatedOnUtc descending
/// 
/// **Pagination:**
/// Default: Page 1, PageSize 10
/// </summary>
public class GetList[Entity]Handler : IQueryHandler<GetList[Entity]Query, [Response]>
```

### Delete Handler Pattern
```csharp
/// <summary>
/// Handler for deleting a [Entity] by ID.
/// 
/// **Responsibility:**
/// Deletes a [Entity] from the database after validation.
/// 
/// **Pre-Delete Validation:**
/// - Checks for dependent records in: [related entity 1], [related entity 2]
/// - Prevents deletion if: [business rule 1], [business rule 2]
/// 
/// **Cascading Effects:**
/// - Related records: [what happens to them]
/// 
/// **Permissions:**
/// Requires: [Module].[Entity].Delete
/// </summary>
public class Delete[Entity]Handler : ICommandHandler<Delete[Entity]Command>
```

### Update Handler Pattern
```csharp
/// <summary>
/// Handler for updating a [Entity].
/// 
/// **Responsibility:**
/// Updates an existing [Entity] with new values after validation.
/// 
/// **Updateable Fields:**
/// - [Field 1]: [constraints]
/// - [Field 2]: [constraints]
/// 
/// **Immutable Fields:**
/// - [Field]: Cannot be changed after creation
/// 
/// **Permissions:**
/// Requires: [Module].[Entity].Edit
/// </summary>
public class Update[Entity]Handler : ICommandHandler<Update[Entity]Command>
```

## Validator Documentation

Every FluentValidation validator should include:

```csharp
/// <summary>
/// Validator for [Command/Query] command.
/// 
/// **Rules:**
/// - [Rule name]: [Rule description]
/// - [Rule name]: [Rule description with parameters]
/// 
/// **Related Exceptions:**
/// Validation failures throw BadRequestException via middleware.
/// </summary>
public class [ValidatorName] : AbstractValidator<[CommandName]>
```

## Documentation Content Guidelines

### Do's
✅ **Be specific** - Explain WHAT it does, not just that it's a handler
✅ **Include constraints** - Document any business rules or limitations
✅ **Document side effects** - List any domain events or cascading operations
✅ **Explain returns** - Be clear about what is returned on success
✅ **Note dependencies** - Mention required permissions or related entities
✅ **Include examples of failure** - Document common error cases

### Don'ts
❌ **Don't be generic** - Avoid "This is a handler"
❌ **Don't assume knowledge** - Explain context for complex operations
❌ **Don't ignore validation** - Document what's being validated and why
❌ **Don't skip error cases** - New developers need to know failure modes
❌ **Don't make up details** - Stick to what the code actually does

## Example: Complete Handler Documentation

```csharp
namespace FSH.Module.Accounting.Features.v1.Invoices.CreateInvoice;

/// <summary>
/// Create Invoice command.
/// 
/// **Purpose:**
/// Creates a new invoice (AR or AP) in the accounting system. Supports both
/// customer invoices (Accounts Receivable) and vendor invoices (Accounts Payable).
/// The invoice starts in Draft status and must be manually approved before posting.
/// 
/// **Parameters:**
/// - InvoiceNumber: Unique identifier (must be unique per tenant and type)
/// - InvoiceDate: Date the invoice was issued
/// - InvoiceType: "AR" for customer invoice or "AP" for vendor invoice
/// - DueDate: Payment due date (must be >= InvoiceDate)
/// - CustomerId: Required for AR invoices, must reference valid Customer
/// - VendorId: Required for AP invoices, must reference valid Vendor
/// 
/// **Multi-Tenancy:**
/// Tenant is automatically assigned from current user context.
/// All references (Customer, Vendor, ChartOfAccount) must exist in same tenant.
/// 
/// **Validation:**
/// CreateInvoiceCommandValidator ensures:
/// - InvoiceNumber is not empty and unique per tenant
/// - InvoiceType is either "AR" or "AP"
/// - AR invoices have CustomerId; AP invoices have VendorId
/// - DueDate >= InvoiceDate
/// - Line items total matches SubTotal
/// - Tax and discount amounts are non-negative
/// </summary>
public record CreateInvoiceCommand(
    string InvoiceNumber,
    DateTime InvoiceDate,
    string InvoiceType,
    DateTime DueDate,
    Guid? CustomerId = null,
    Guid? VendorId = null,
    string? Description = null) : ICommand<Guid>;

/// <summary>
/// Handler for creating a new Invoice.
/// 
/// **Responsibility:**
/// Creates a new Invoice entity with provided properties and persists to database.
/// Establishes initial Draft status and audit trail.
/// 
/// **Execution Flow:**
/// 1. Validate command via fluent validator
/// 2. Call Invoice.Create() factory with command properties
/// 3. Add to DbContext.Invoices
/// 4. Call SaveChangesAsync() to persist
/// 5. Return new invoice ID
/// 
/// **Permissions:**
/// Requires: Accounting.Invoices.Create
/// 
/// **Return Value:**
/// Returns the newly created invoice's ID (Guid)
/// 
/// **Exceptions:**
/// - BadRequestException: If validation fails
/// - ValidationException: If command properties are invalid
/// </summary>
public class CreateInvoiceHandler(AccountingDbContext context, ICurrentUser currentUser)
    : ICommandHandler<CreateInvoiceCommand, Guid>
{
    /// <summary>
    /// Handles the create invoice request.
    /// </summary>
    /// <param name="command">Create invoice command with invoice details</param>
    /// <param name="ct">Cancellation token for async operation</param>
    /// <returns>The ID of the newly created invoice</returns>
    /// <exception cref="BadRequestException">If invoice creation fails validation</exception>
    public async ValueTask<Guid> Handle(CreateInvoiceCommand command, CancellationToken ct)
    {
        var entity = Invoice.Create(
            command.InvoiceNumber,
            command.InvoiceDate,
            command.InvoiceType,
            command.DueDate,
            command.CustomerId,
            command.VendorId,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);

        context.Invoices.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
```

## Implementation Strategy

For Phase 2 documentation:

1. **Priority 1 (Core Operations)**: Document Create, Get, GetList, Delete, Update handlers for primary entities (Invoice, ChartOfAccount, JournalEntry, etc.)
2. **Priority 2 (Business Logic)**: Document complex operations (Post, Approve, Close, Reconcile, etc.)
3. **Priority 3 (Supporting)**: Document secondary entities and utilities

This systematic approach ensures the most critical handlers are well-documented first, providing maximum value for new developers.

## Validation Documentation Pattern

```csharp
/// <summary>
/// Validates [Command] command.
/// 
/// **Rules Enforced:**
/// - [Property]: [Must/Must Not] [condition]
/// - [Property]: [Pattern/Range] [constraint]
/// - [Property]: [Custom Rule] [logic]
/// 
/// **Error Messages:**
/// All validation failures produce user-friendly error messages
/// indicating the specific field and constraint that failed.
/// </summary>
public class [ValidatorName] : AbstractValidator<[CommandName]>
{
    public [ValidatorName]()
    {
        RuleFor(x => x.PropertyName)
            .NotEmpty().WithMessage("Property Name is required")
            .MaximumLength(100).WithMessage("Property Name must not exceed 100 characters");
    }
}
```

---

**Note**: All documentation should be kept in sync with code changes. When updating handler logic or validation rules, update the corresponding XML documentation comments.
