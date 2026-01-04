# Accounting Module Restructuring Guide

## Status
The Accounting module has been temporarily removed from the solution file (`FSH.Framework.slnx`) while being restructured to match FSH's vertical slice architecture. 

**Current Issue:** The module was copied from a clean architecture solution and needs significant refactoring to align with FSH patterns.

## Solution Structure Issues Found

1. **DTO Records Missing Type Information**
   - All DTO records are incomplete (e.g., `public record CostCenterDto(Guid Id, Code, Name, ...);`)
   - Missing type annotations for properties
   - Example error: `error CS1001: Identifier expected`

2. **Mixed Architecture Patterns**
   - Old clean architecture (Accounting.Domain, Accounting.Application, Accounting.Infrastructure)
   - Conflicting with FSH's vertical slice single-project approach
   - Removed old projects; keeping only `Module.Accounting` and `Module.Accounting.Contracts`

## FSH Vertical Slice Architecture Pattern

### Project Structure
```
Modules/Accounting/
├── Module.Accounting.Contracts/
│   ├── v1/
│   │   ├── {Feature}/
│   │   │   ├── {Feature}Command.cs (Commands/Queries)
│   │   │   ├── {Feature}Dto.cs (Data Transfer Objects)
│   │   │   └── {Feature}Response.cs (Response objects if needed)
│   │   └── ...
│   └── Global Usings
│
└── Module.Accounting/
    ├── Domain/
    │   ├── Entities/ (Domain models)
    │   ├── Rules/ (Business logic)
    │   └── Events/ (Domain events)
    ├── Data/
    │   ├── AccountingDbContext.cs
    │   └── Configurations/ (EF configurations)
    ├── Features/
    │   └── v1/
    │       └── {Feature}/
    │           ├── {Feature}Command.cs (or Query)
    │           ├── {Feature}CommandHandler.cs
    │           ├── {Feature}CommandValidator.cs
    │           ├── {Feature}Endpoint.cs
    │           └── {Feature}Specification.cs (if using specs)
    ├── Events/ (Domain & integration events)
    ├── AccountingModule.cs (IModule implementation)
    └── Global Usings
```

## How to Properly Restructure Accounting Module

### Phase 1: Fix Contracts Project
All DTOs must have complete type information:

#### ✅ CORRECT Pattern (from Todo Module)
```csharp
namespace FSH.Module.Accounting.Contracts.v1.Customers;

/// <summary>
/// Customer data transfer object.
/// </summary>
public class CustomerDto
{
    /// <summary>
    /// Gets or sets the customer ID.
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Gets or sets the customer code.
    /// </summary>
    public string Code { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer description.
    /// </summary>
    public string? Description { get; init; }
}

public record CreateCustomerCommand(string Code, string Name, string? Description) 
    : ICommand<Guid>;

public record UpdateCustomerCommand(Guid Id, string Code, string Name, string? Description) 
    : ICommand;

public record DeleteCustomerCommand(Guid Id) 
    : ICommand;

public record GetCustomerQuery(Guid Id) 
    : IQuery<CustomerDto>;

public record GetCustomersQuery(int Page = 1, int PageSize = 10) 
    : IQuery<PagedList<CustomerDto>>;
```

#### ❌ INCORRECT Pattern (Current Accounting DTOs)
```csharp
// Missing types and incomplete
public record CostCenterDto(Guid Id, Code, Name, Description, Department, bool IsActive, DateTime CreatedOnUtc);
```

### Phase 2: Domain Entities
Each entity should inherit from FSH's DDD primitives:

```csharp
namespace FSH.Module.Accounting.Domain.Entities;

using FSH.BuildingBlocks.Core.Domain;

/// <summary>
/// Represents a customer in the accounting system.
/// </summary>
public class Customer : AuditableEntity, IAggregateRoot
{
    /// <summary>
    /// Gets or sets the unique code for the customer.
    /// </summary>
    public string Code { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the customer name.
    /// </summary>
    public string Name { get; private set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the optional description.
    /// </summary>
    public string? Description { get; private set; }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="Customer"/> class.
    /// </summary>
    private Customer() { }
    
    /// <summary>
    /// Creates a new customer.
    /// </summary>
    public static Customer Create(string code, string name, string? description = null)
    {
        var customer = new Customer
        {
            Code = code,
            Name = name,
            Description = description
        };
        return customer;
    }
    
    /// <summary>
    /// Updates the customer details.
    /// </summary>
    public void Update(string code, string name, string? description = null)
    {
        Code = code;
        Name = name;
        Description = description;
    }
}
```

### Phase 3: Feature Implementation
Follow the Todo module pattern for each feature:

#### Command Handler
```csharp
namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

public class CreateCustomerCommandHandler : ICommandHandler<CreateCustomerCommand, Guid>
{
    private readonly IRepository<Customer> _repository;
    private readonly IUnitOfWork _unitOfWork;
    
    public CreateCustomerCommandHandler(
        IRepository<Customer> repository,
        IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async ValueTask<Guid> Handle(
        CreateCustomerCommand request,
        CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.Code, request.Name, request.Description);
        await _repository.AddAsync(customer, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return customer.Id;
    }
}
```

#### Validator
```csharp
namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
    public CreateCustomerCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);
            
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(255);
    }
}
```

#### Endpoint
```csharp
namespace FSH.Module.Accounting.Features.v1.Customers.CreateCustomer;

public static class CreateCustomerEndpoint
{
    public static RouteHandlerBuilder MapCreateCustomerEndpoint(
        this IEndpointRouteBuilder endpoints)
    {
        return endpoints
            .MapPost("/customers", Handle)
            .WithName(nameof(CreateCustomerEndpoint))
            .WithSummary("Create a new customer")
            .WithOpenApi()
            .Produces<IdResponse>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .RequireAuthorization();
    }
    
    private static async Task<IResult> Handle(
        CreateCustomerCommand command,
        IMediator mediator,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return TypedResults.Created($"/customers/{result}", new IdResponse(result));
    }
}
```

### Phase 4: DbContext Configuration
```csharp
namespace FSH.Module.Accounting.Data;

using FSH.Module.Accounting.Domain.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext for the Accounting module.
/// </summary>
public class AccountingDbContext : DbContext
{
    public AccountingDbContext(DbContextOptions<AccountingDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vendor> Vendors => Set<Vendor>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);
    }
}
```

### Phase 5: Module Registration
```csharp
namespace FSH.Module.Accounting;

using FSH.BuildingBlocks.Core.Modularity;
using FSH.Module.Accounting.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

/// <summary>
/// Accounting module registration.
/// </summary>
public class AccountingModule : IModule
{
    public IServiceCollection ConfigureServices(
        IServiceCollection services,
        IHostApplicationBuilder builder)
    {
        // Register DbContext
        services.AddDbContext<AccountingDbContext>((provider, options) =>
        {
            var connectionString = builder.Configuration
                .GetConnectionString("DefaultConnection");
            options.UseNpgsql(connectionString);
        });
        
        // Register repositories
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        
        // Register validators
        services.AddValidatorsFromAssemblyContaining<typeof(AccountingModule)>();
        
        return services;
    }
    
    public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var group = endpoints
            .MapGroup("/api/v1/accounting")
            .WithTags("Accounting");
        
        group.MapCreateCustomerEndpoint();
        group.MapGetCustomersEndpoint();
        group.MapUpdateCustomerEndpoint();
        group.MapDeleteCustomerEndpoint();
        
        group.MapCreateVendorEndpoint();
        group.MapGetVendorsEndpoint();
        
        return endpoints;
    }
}
```

## Next Steps

1. **Review the Todo Module** (working reference implementation):
   - `/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Todos/`
   - Study the feature structure and patterns

2. **Fix Accounting DTOs First**:
   - Add complete type information to all DTO record properties
   - Add proper XML documentation comments
   - Follow the Customer/Vendor patterns already partially created

3. **Clean Up Domain**:
   - Remove any references to the old clean architecture patterns
   - Use FSH's `AuditableEntity` and `IAggregateRoot` base classes
   - Create proper domain entity constructors and factory methods

4. **Reorganize Features**:
   - Ensure each feature has: Command/Query, Handler, Validator, Endpoint
   - Group by entity (Customers, Vendors, etc.)
   - Follow version naming: v1/v2 for API versioning

5. **Re-add to Solution**:
   - Once fixed, add back to `FSH.Framework.slnx`
   - Verify the build succeeds
   - Run tests

## Important Notes

- **No Duplicate Domains Conflict**: Each module can have its own domain entities. FSH's vertical slice architecture explicitly allows multiple modules to define similar domains independently.
- **Single Project Approach**: All features for Accounting live in one `Module.Accounting` project, not separate Domain/Application/Infrastructure projects.
- **Contracts Separation**: Public DTOs and commands go in `Module.Accounting.Contracts` for client consumption.
- **Focus on Simplicity**: Don't over-engineer with CQRS, complex patterns, or unnecessary abstraction layers beyond what FSH provides.

## Reference Files
- Todo Module Implementation: Check `/src/Modules/Todos/` for the working example
- COPILOT_INSTRUCTIONS.md: Core patterns and conventions
- FSH Architecture Guide: `/ARCHITECTURE_GUIDE.md`
