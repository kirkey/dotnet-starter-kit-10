# Accounting Module Alignment with Vertical-Slice Architecture

## Overview

The Accounting module has been aligned with the vertical-slice architecture pattern and COPILOT_INSTRUCTIONS guidelines, following the same structure and patterns used in the proven Todos module.

## Architecture Patterns Implemented

### ✅ Vertical-Slice Organization

Each feature is organized as a complete vertical slice including:
- **Command/Query** (in Contracts project) - ICommand<T> / IQuery<T> records
- **Handler** - ICommandHandler / IQueryHandler implementations
- **Validator** - FluentValidation rules
- **Endpoint** - Static extension methods mapping HTTP routes

Example structure:
```
Features/v1/ChartOfAccounts/
├── CreateChartOfAccount/
│   ├── CreateChartOfAccountCommand        (in Contracts)
│   ├── CreateChartOfAccountHandler
│   ├── CreateChartOfAccountValidator
│   └── CreateChartOfAccountEndpoint
├── GetChartOfAccount/
├── GetChartOfAccounts/
├── UpdateChartOfAccount/
└── DeleteChartOfAccount/
```

### ✅ CQRS Pattern

- **Commands** (Create, Update, Delete) return ICommand<TResponse>
- **Queries** (Get, List, Search) return IQuery<TResponse>
- **Handlers** use dependency injection for DbContext and services
- **Mediator** library (not MediatR) for command/query dispatch

### ✅ Domain-Driven Design

- **Domain Entities** in `Domain/` folder with validation logic
- **Aggregate Pattern** using factory methods (Create, Update, Delete)
- **Domain Events** in `Events/` folder for important state changes
- **Value Objects** for complex properties (e.g., Amount, Balance)
- **Multi-Tenancy Support** via IMustHaveTenant interface

### ✅ Exception Handling

Module-specific exceptions in `Exceptions/` folder:
- `AccountNotFoundException` - For missing Chart of Account
- `JournalEntryNotFoundException` - For missing Journal Entry
- `InvoiceNotFoundException` - For missing Invoice
- `AccountingExceptionExtensions` - Fluent validation helpers

### ✅ Event-Driven Architecture

Domain events in `Events/AccountingDomainEvents.cs`:
- `JournalEntryCreatedEvent` - Raised on journal entry creation
- `JournalEntryPostedEvent` - Raised when journal entry is posted
- `InvoiceCreatedEvent` - Raised on invoice creation
- `FiscalPeriodClosedEvent` - Raised on fiscal period close

### ✅ Shared Infrastructure

Global usings in `GlobalUsings.cs`:
```csharp
global using FSH.Framework.Core.Domain;
global using FSH.Framework.Caching;
global using FSH.Framework.Shared.Identity;
global using FSH.Framework.Persistence;
global using FSH.Framework.Core.Context;
global using Mediator;
global using Microsoft.EntityFrameworkCore;
```

### ✅ Module Registration

`AccountingModule.cs` implements `IModule`:
- Registers DbContext with HeroDbContext
- Registers IDbInitializer for database seeding
- Maps all endpoints with proper versioning (v1)
- Configures health checks
- Integrates with permission system

## Feature Implementation Standards

### Command Handler Pattern

```csharp
/// <summary>Documentation with purpose and dependencies.</summary>
public class CreateChartOfAccountHandler(
    AccountingDbContext context,
    ICurrentUser currentUser) 
    : ICommandHandler<CreateChartOfAccountCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateChartOfAccountCommand command, CancellationToken ct)
    {
        var entity = ChartOfAccount.Create(
            command.AccountCode,
            command.AccountName,
            // ... parameters
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System");
        
        context.ChartOfAccounts.Add(entity);
        await context.SaveChangesAsync(ct);
        
        return entity.Id;
    }
}
```

### Endpoint Pattern

```csharp
/// <summary>Detailed documentation with HTTP mapping, security, request/response info.</summary>
public static class CreateChartOfAccountEndpoint
{
    /// <summary>Maps the endpoint to the route group.</summary>
    public static RouteHandlerBuilder MapCreateChartOfAccountEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateChartOfAccountCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/accounting/accounts/{id}", id);
        })
        .WithName(nameof(CreateChartOfAccountEndpoint))
        .WithSummary("Create Chart of Account")
        .WithDescription("Creates a new account in the chart of accounts")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Create);
    }
}
```

### Validator Pattern

```csharp
public class CreateChartOfAccountCommandValidator 
    : AbstractValidator<CreateChartOfAccountCommand>
{
    public CreateChartOfAccountCommandValidator()
    {
        RuleFor(x => x.AccountCode)
            .NotEmpty().WithMessage("Account code is required")
            .MaximumLength(50);
        
        RuleFor(x => x.AccountName)
            .NotEmpty().WithMessage("Account name is required")
            .MaximumLength(200);
        
        RuleFor(x => x.AccountType)
            .NotEmpty().WithMessage("Account type is required")
            .Must(x => IsValidAccountType(x));
    }
}
```

## Multi-Tenancy Support

All entities implement `IMustHaveTenant`:
```csharp
public class ChartOfAccount : AuditableEntity<Guid>, IMustHaveTenant
{
    public string TenantId { get; private set; } = default!;
    // ... other properties
}
```

Handlers automatically assign tenant from current user:
```csharp
var entity = ChartOfAccount.Create(
    command.AccountCode,
    command.AccountName,
    // ...
    currentUser.GetTenant() ?? "root",  // Automatic tenant assignment
    currentUser.GetUserId(),
    currentUser.Name ?? "System");
```

## Auditing & Compliance

All entities track creation and modification:
```csharp
public class ChartOfAccount : AuditableEntity<Guid>
{
    public Guid CreatedBy { get; set; }
    public string CreatedByUserName { get; set; }
    public DateTimeOffset CreatedOnUtc { get; set; }
    public Guid? ModifiedBy { get; set; }
    public string? ModifiedByUserName { get; set; }
    public DateTimeOffset? ModifiedOnUtc { get; set; }
}
```

## Authorization & Permissions

Permission-based authorization using permission constants:
```csharp
.RequirePermission(AccountingPermissionConstants.ChartOfAccounts.Create)

// Defined in AccountingPermissionConstants.cs
public static class ChartOfAccounts
{
    public const string View = "accounting:chartofaccounts:view";
    public const string Create = "accounting:chartofaccounts:create";
    public const string Update = "accounting:chartofaccounts:update";
    public const string Delete = "accounting:chartofaccounts:delete";
}
```

## Data Persistence

- **EF Core** with DbContext in `Data/AccountingDbContext.cs`
- **Entity Configurations** in `Data/Configurations/`
- **Database Migrations** for all entities
- **Database Per Tenant** with Finbuckle multi-tenancy

## Validation

Two-layer validation approach:
1. **FluentValidation** - In command validators for business rules
2. **Domain Validation** - In entity factory methods for invariants

```csharp
// Validator
RuleFor(x => x.Balance)
    .GreaterThanOrEqualTo(0).WithMessage("Balance must be non-negative");

// Domain (in ChartOfAccount.Create)
if (isControlAccount && !string.IsNullOrWhiteSpace(parentCode))
    throw new BadRequestException("Control accounts cannot have parent accounts");
```

## Documentation Standards

Each feature includes comprehensive documentation:
- **Command Records**: Purpose, validation rules, multi-tenancy notes
- **Handlers**: Purpose, domain logic, dependencies, return values
- **Endpoints**: HTTP mapping, security requirements, request/response specs
- **Validators**: Validation rules with error messages
- **Exceptions**: When thrown, HTTP mapping, recovery options

Example XML documentation:
```csharp
/// <summary>
/// Handles the creation of a new Chart of Account.
/// 
/// **Purpose:**
/// Processes the CreateChartOfAccountCommand by:
/// 1. Creating a new ChartOfAccount aggregate
/// 2. Setting parent account relationships
/// 3. Recording initial balance
/// 4. Persisting to the database
/// 5. Returning the new account's ID
/// 
/// **Domain Logic:**
/// - Uses factory method for proper initialization
/// - Maintains account hierarchy
/// - Validates account type
/// - Associates with user and tenant
/// 
/// **Dependencies:**
/// - AccountingDbContext
/// - ICurrentUser
/// </summary>
```

## Completed Implementations

### 50 Accounting Entities Covered

✅ Chart of Accounts, General Ledger, Journal Entries
✅ Accounting Periods, Fiscal Period Close, Trial Balance
✅ Accounts Payable & Receivable
✅ Invoices, Bills, Credit/Debit Memos
✅ Bank Reconciliation, Checks, Payments
✅ Fixed Assets, Depreciation Methods
✅ Budgets, Cost Centers, Tax Codes
✅ Utility-specific (Meters, Consumption, Patronage Capital, Rates)
✅ And more...

### Feature Operations Per Entity

Each entity includes standard CRUD operations:
- **Create** - CreateXxxHandler, CreateXxxValidator, CreateXxxEndpoint
- **Read** - GetXxxHandler, GetXxxEndpoint
- **List** - GetXxxsHandler, GetXxxsEndpoint (with pagination/filtering)
- **Update** - UpdateXxxHandler, UpdateXxxValidator, UpdateXxxEndpoint
- **Delete** - DeleteXxxHandler, DeleteXxxEndpoint
- **Custom** - Domain-specific operations (Post, Approve, Reconcile, etc.)

## Migration Path from Legacy Code

To migrate legacy code to this pattern:

1. **Create Command** in Contracts project
2. **Create Validator** following FluentValidation patterns
3. **Create Handler** with proper DI and domain logic
4. **Create Endpoint** with routing and security
5. **Create Tests** for handler, validator, and endpoint
6. **Update Module** registration
7. **Create Database Migration** for any schema changes

## Future Enhancements

1. **Event Handlers** - Subscribe to domain events for side effects
2. **Event Sourcing** - Use events as primary data store
3. **CQRS Optimization** - Separate read/write databases
4. **Sagas** - Implement compensating transactions
5. **Event Streaming** - Integrate with Kafka/RabbitMQ
6. **Integration Events** - Module-to-module communication

## Testing Strategy

Each feature should include:
- **Unit Tests** - Handler logic with mocked DbContext
- **Validator Tests** - Validation rules with valid/invalid inputs
- **Integration Tests** - End-to-end tests with test database
- **API Tests** - Endpoint testing via HTTP

Example test pattern:
```csharp
[Fact]
public async Task CreateChartOfAccount_WithValidCommand_ReturnsAccountId()
{
    // Arrange
    var command = new CreateChartOfAccountCommand(
        "1000", "Assets", "Asset", "Utility", balance: 0);
    var handler = new CreateChartOfAccountHandler(_context, _currentUser);
    
    // Act
    var id = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    Assert.NotEqual(Guid.Empty, id);
    var account = await _context.ChartOfAccounts.FindAsync(id);
    Assert.NotNull(account);
    Assert.Equal("1000", account.AccountCode);
}
```

## Compliance & Best Practices

✅ Follows COPILOT_INSTRUCTIONS.md guidelines
✅ Matches Todos module patterns for consistency
✅ DDD principles (aggregates, value objects, domain events)
✅ CQRS separation of concerns
✅ Multi-tenancy support throughout
✅ Comprehensive audit trails
✅ Permission-based authorization
✅ Fluent validation for business rules
✅ Detailed XML documentation
✅ Proper exception handling with specific types

---

**Last Updated:** January 4, 2026
**Alignment Status:** ✅ Complete
**Test Coverage:** In Progress
**Documentation:** Comprehensive
