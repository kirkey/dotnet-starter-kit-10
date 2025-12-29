# Copilot Agent Instructions - FSH .NET 10 Starter Kit

> **IMPORTANT**: Before making ANY changes to this codebase, you MUST read and follow these instructions. This ensures code consistency, maintainability, and adherence to architectural patterns.

## 📋 Quick Reference Checklist

When making changes, ask yourself:

- [ ] Have I read the relevant pattern in this document?
- [ ] Does my change follow the vertical slice architecture?
- [ ] Am I placing files in the correct directory structure?
- [ ] Have I followed naming conventions?
- [ ] Have I added proper validation using FluentValidation?
- [ ] Have I used the Mediator pattern (not MediatR)?
- [ ] Have I ensured multi-tenancy support (IMustHaveTenant)?
- [ ] Have I added proper error handling?
- [ ] Have I followed the endpoint pattern (static extension methods)?
- [ ] Have I updated documentation if needed?

---

## 🏗️ Architecture Overview

### Core Principles

1. **Modular Monolith**: Modules are loosely coupled and can be extracted to microservices
2. **Vertical Slice Architecture**: Features are organized by business capability, not technical layers
3. **CQRS**: Commands (write) and Queries (read) are separated
4. **Multi-Tenancy**: Database-per-tenant isolation using Finbuckle
5. **Event-Driven**: Modules communicate via integration events (Inbox/Outbox pattern)

### Repository Structure

```
src/
├── BuildingBlocks/       # Reusable infrastructure (Core, Persistence, Web, Caching, etc.)
├── Modules/             # Business domain modules (Identity, Multitenancy, Auditing)
├── Playground/          # Reference implementation (API, Blazor, Migrations)
└── Tests/               # Architecture tests (enforce architectural rules)
```

---

## 📐 Module Structure Pattern

### Directory Layout

Every module MUST follow this structure:

```
Modules/{ModuleName}/
├── Modules.{ModuleName}.Contracts/      # Public DTOs (shared with clients)
│   ├── Modules.{ModuleName}.Contracts.csproj
│   ├── v1/                              # API version
│   │   └── {Feature}/
│   │       ├── {Feature}Command.cs      # Request DTO (ICommand<TResponse>)
│   │       ├── {Feature}Query.cs        # Query DTO (IQuery<TResponse>)
│   │       └── {Feature}Response.cs     # Response DTO
│   └── DTOs/                            # Shared DTOs
│
└── Modules.{ModuleName}/                # Implementation (internal)
    ├── Modules.{ModuleName}.csproj
    ├── {ModuleName}Module.cs            # Module registration (IModule)
    ├── Domain/                          # Domain entities
    │   └── {Entity}.cs
    ├── Data/                            # EF Core DbContext
    │   ├── {ModuleName}DbContext.cs
    │   └── Configurations/              # Entity configurations
    │       └── {Entity}Configuration.cs
    ├── Features/                        # Vertical slices by version
    │   └── v1/
    │       └── {Feature}/
    │           ├── {Feature}CommandHandler.cs    # Handler
    │           ├── {Feature}CommandValidator.cs  # FluentValidation
    │           └── {Feature}Endpoint.cs          # Minimal API endpoint
    ├── Events/                          # Domain/Integration events
    ├── Services/                        # Domain services
    └── Authorization/                   # Permissions & policies
```

---

## 🎯 Feature Implementation Pattern (CQRS)

### Command Pattern (Write Operations)

#### 1. Command (in Contracts project)

```csharp
// File: Modules.{Module}.Contracts/v1/{Feature}/{Feature}Command.cs
using Mediator;

namespace FSH.Modules.{Module}.Contracts.v1.{Feature};

/// <summary>
/// Command to {describe action}.
/// </summary>
public record {Feature}Command(
    string Property1,
    int Property2
) : ICommand<Guid>; // Return type (Guid, Unit, custom response)
```

#### 2. Validator (in Module project)

```csharp
// File: Modules.{Module}/Features/v1/{Feature}/{Feature}CommandValidator.cs
using FluentValidation;
using FSH.Modules.{Module}.Contracts.v1.{Feature};

namespace FSH.Modules.{Module}.Features.v1.{Feature};

public class {Feature}CommandValidator : AbstractValidator<{Feature}Command>
{
    public {Feature}CommandValidator()
    {
        RuleFor(x => x.Property1)
            .NotEmpty().WithMessage("Property1 is required")
            .MaximumLength(200);
        
        RuleFor(x => x.Property2)
            .GreaterThan(0).WithMessage("Property2 must be positive");
    }
}
```

#### 3. Handler (in Module project)

```csharp
// File: Modules.{Module}/Features/v1/{Feature}/{Feature}CommandHandler.cs
using FSH.Framework.Core.Context;
using FSH.Modules.{Module}.Contracts.v1.{Feature};
using FSH.Modules.{Module}.Data;
using FSH.Modules.{Module}.Domain;
using Mediator;

namespace FSH.Modules.{Module}.Features.v1.{Feature};

public class {Feature}CommandHandler : ICommandHandler<{Feature}Command, Guid>
{
    private readonly {Module}DbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public {Feature}CommandHandler({Module}DbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }
    
    public async ValueTask<Guid> Handle({Feature}Command command, CancellationToken ct)
    {
        // 1. Create entity
        var entity = new {Entity}
        {
            Id = Guid.NewGuid(),
            Property1 = command.Property1,
            Property2 = command.Property2,
            TenantId = _currentUser.TenantId!, // Multi-tenancy
            CreatedBy = _currentUser.UserId,
            CreatedOn = DateTimeOffset.UtcNow
        };
        
        // 2. Add to context
        _db.{Entities}.Add(entity);
        
        // 3. Save changes (triggers domain events if any)
        await _db.SaveChangesAsync(ct);
        
        // 4. Return result
        return entity.Id;
    }
}
```

#### 4. Endpoint (in Module project)

```csharp
// File: Modules.{Module}/Features/v1/{Feature}/{Feature}Endpoint.cs
using FSH.Modules.{Module}.Contracts.v1.{Feature};
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.{Module}.Features.v1.{Feature};

public static class {Feature}Endpoint
{
    public static RouteHandlerBuilder Map{Feature}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{resource-path}",
            async Task<Results<Created<Guid>, ValidationProblem>> 
            ({Feature}Command command, IMediator mediator, CancellationToken ct) =>
            {
                var id = await mediator.Send(command, ct);
                return TypedResults.Created($"/{resource-path}/{id}", id);
            })
            .WithName("{Feature}")
            .WithSummary("Brief summary of what this endpoint does")
            .WithDescription("Detailed description")
            .WithTags("{Module}")
            .RequirePermission("{module}:{resource}:create") // Use RequireAuthorization() for simple auth
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}
```

### Query Pattern (Read Operations)

#### 1. Query (in Contracts project)

```csharp
// File: Modules.{Module}.Contracts/v1/{Feature}/{Feature}Query.cs
using Mediator;

namespace FSH.Modules.{Module}.Contracts.v1.{Feature};

public record {Feature}Query(
    Guid Id
) : IQuery<{Feature}Response>;

public record {Feature}Response(
    Guid Id,
    string Property1,
    int Property2,
    DateTimeOffset CreatedOn
);
```

#### 2. Handler (in Module project)

```csharp
// File: Modules.{Module}/Features/v1/{Feature}/{Feature}QueryHandler.cs
using FSH.Modules.{Module}.Contracts.v1.{Feature};
using FSH.Modules.{Module}.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.{Module}.Features.v1.{Feature};

public class {Feature}QueryHandler : IQueryHandler<{Feature}Query, {Feature}Response?>
{
    private readonly {Module}DbContext _db;
    
    public {Feature}QueryHandler({Module}DbContext db)
    {
        _db = db;
    }
    
    public async ValueTask<{Feature}Response?> Handle({Feature}Query query, CancellationToken ct)
    {
        return await _db.{Entities}
            .Where(e => e.Id == query.Id)
            .Select(e => new {Feature}Response(
                e.Id,
                e.Property1,
                e.Property2,
                e.CreatedOn))
            .FirstOrDefaultAsync(ct);
    }
}
```

#### 3. Endpoint (in Module project)

```csharp
// File: Modules.{Module}/Features/v1/{Feature}/{Feature}Endpoint.cs
public static class {Feature}Endpoint
{
    public static RouteHandlerBuilder Map{Feature}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{resource-path}/{id:guid}",
            async Task<Results<Ok<{Feature}Response>, NotFound>> 
            (Guid id, IMediator mediator, CancellationToken ct) =>
            {
                var result = await mediator.Send(new {Feature}Query(id), ct);
                return result is not null 
                    ? TypedResults.Ok(result) 
                    : TypedResults.NotFound();
            })
            .WithName("Get{Feature}")
            .WithSummary("Get {feature} by ID")
            .RequirePermission("{module}:{resource}:read")
            .Produces<{Feature}Response>()
            .Produces(StatusCodes.Status404NotFound);
    }
}
```

---

## 🗃️ Domain Entity Pattern

### Basic Entity with Multi-Tenancy and Auditing

```csharp
// File: Modules.{Module}/Domain/{Entity}.cs
using FSH.Framework.Shared.Persistence;

namespace FSH.Modules.{Module}.Domain;

/// <summary>
/// Represents a {entity description}.
/// </summary>
public class {Entity} : IAuditableEntity, IMustHaveTenant
{
    // Primary Key
    public Guid Id { get; set; }
    
    // Business Properties
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    
    // Multi-Tenancy (REQUIRED for tenant isolation)
    public string TenantId { get; set; } = default!;
    
    // Audit Trail (auto-populated by framework)
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    
    // Navigation properties (if needed)
    // public virtual ICollection<RelatedEntity> RelatedEntities { get; set; } = new List<RelatedEntity>();
}
```

### Entity with Domain Events (Advanced)

```csharp
using FSH.Framework.Core.Domain;

public class Order : AggregateRoot, IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = default!;
    public OrderStatus Status { get; private set; }
    public decimal TotalAmount { get; set; }
    
    // State transition with domain event
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Cannot confirm order in current state");
        
        Status = OrderStatus.Confirmed;
        
        // Raise domain event
        RaiseDomainEvent(new OrderConfirmedEvent(Id, OrderNumber));
    }
    
    // Required for multi-tenancy and auditing
    public string TenantId { get; set; } = default!;
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
```

---

## 💾 DbContext Pattern

### Module DbContext

```csharp
// File: Modules.{Module}/Data/{Module}DbContext.cs
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Eventing.Inbox;
using FSH.Framework.Eventing.Outbox;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.{Module}.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FSH.Modules.{Module}.Data;

public class {Module}DbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;
    
    // DbSets
    public DbSet<{Entity}> {Entities} => Set<{Entity}>();
    
    // Required for Outbox/Inbox pattern
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    
    public {Module}DbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<{Module}DbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment) : base(options)
    {
        _environment = environment;
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        // Apply entity configurations
        builder.ApplyConfigurationsFromAssembly(typeof({Module}DbContext).Assembly);
        
        // Configure Outbox/Inbox for event-driven architecture
        builder.ApplyConfiguration(new OutboxMessageConfiguration("{module}"));
        builder.ApplyConfiguration(new InboxMessageConfiguration("{module}"));
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Configure tenant-specific connection
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

### Entity Configuration

```csharp
// File: Modules.{Module}/Data/Configurations/{Entity}Configuration.cs
using FSH.Modules.{Module}.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.{Module}.Data.Configurations;

public class {Entity}Configuration : IEntityTypeConfiguration<{Entity}>
{
    public void Configure(EntityTypeBuilder<{Entity}> builder)
    {
        // Table mapping
        builder.ToTable("{Entities}", "{module}"); // Schema: {module}
        
        // Primary key
        builder.HasKey(x => x.Id);
        
        // Properties
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
        
        // Indexes (for performance)
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Name);
        
        // Relationships (if any)
        // builder.HasMany(x => x.RelatedEntities)
        //     .WithOne()
        //     .HasForeignKey(x => x.{Entity}Id);
    }
}
```

---

## 🔌 Module Registration Pattern

### Module Class (IModule Implementation)

```csharp
// File: Modules.{Module}/{Module}Module.cs
using Asp.Versioning;
using FSH.Framework.Persistence;
using FSH.Framework.Web.Modules;
using FSH.Modules.{Module}.Data;
using FSH.Modules.{Module}.Features.v1.{Feature};
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.{Module};

public class {Module}Module : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register DbContext
        builder.Services.AddHeroDbContext<{Module}DbContext>();
        
        // Register services
        builder.Services.AddScoped<I{Module}Service, {Module}Service>();
        
        // Register eventing (for Outbox/Inbox pattern)
        builder.Services.AddEventingCore(builder.Configuration);
        builder.Services.AddEventingForDbContext<{Module}DbContext>();
        
        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<{Module}DbContext>(
                name: "db:{module}",
                failureStatus: HealthStatus.Unhealthy);
    }
    
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        // API versioning
        var versionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        // Route group
        var group = endpoints.MapGroup("/api/v{version:apiVersion}/{module}")
            .WithApiVersionSet(versionSet)
            .WithTags("{Module}");
        
        // Map all endpoints
        group.Map{Feature1}Endpoint();
        group.Map{Feature2}Endpoint();
        // Add more endpoints...
    }
}
```

---

## 🔐 Authorization Patterns

### Permission-Based Authorization

```csharp
// 1. Define permissions (in Module project)
public static class {Module}PermissionConstants
{
    public const string Prefix = "{module}";
    
    public static class {Resource}
    {
        public const string View = $"{Prefix}:{resource}:view";
        public const string Create = $"{Prefix}:{resource}:create";
        public const string Update = $"{Prefix}:{resource}:update";
        public const string Delete = $"{Prefix}:{resource}:delete";
    }
}

// 2. Use in endpoint
.RequirePermission({Module}PermissionConstants.{Resource}.Create)

// 3. Simple authorization (just authenticated)
.RequireAuthorization()
```

---

## 📦 Naming Conventions

### Files and Classes

| Type | Pattern | Example |
|------|---------|---------|
| Command | `{Feature}Command.cs` | `CreateProductCommand.cs` |
| Query | `{Feature}Query.cs` | `GetProductsQuery.cs` |
| Handler | `{Feature}CommandHandler.cs` | `CreateProductCommandHandler.cs` |
| Validator | `{Feature}CommandValidator.cs` | `CreateProductCommandValidator.cs` |
| Endpoint | `{Feature}Endpoint.cs` | `CreateProductEndpoint.cs` |
| Response | `{Feature}Response.cs` | `ProductResponse.cs` |
| Entity | `{Entity}.cs` | `Product.cs` |
| DbContext | `{Module}DbContext.cs` | `ProductsDbContext.cs` |
| Configuration | `{Entity}Configuration.cs` | `ProductConfiguration.cs` |
| Module | `{Module}Module.cs` | `ProductsModule.cs` |

### Namespaces

```csharp
// Contracts
FSH.Modules.{Module}.Contracts.v1.{Feature}

// Implementation
FSH.Modules.{Module}.Features.v1.{Feature}
FSH.Modules.{Module}.Domain
FSH.Modules.{Module}.Data
FSH.Modules.{Module}.Data.Configurations
```

### Endpoints

```csharp
// Endpoint method naming
public static RouteHandlerBuilder Map{Feature}Endpoint(...)

// Route naming
.WithName("{Feature}")              // e.g., "CreateProduct"
.WithTags("{Module}")               // e.g., "Products"
```

---

## ✅ Validation Rules

### FluentValidation Best Practices

```csharp
public class {Feature}CommandValidator : AbstractValidator<{Feature}Command>
{
    public {Feature}CommandValidator()
    {
        // Required fields
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .MaximumLength(200).WithMessage("Name cannot exceed 200 characters");
        
        // Optional fields with constraints
        RuleFor(x => x.Description)
            .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description));
        
        // Numeric validation
        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be positive")
            .LessThan(1_000_000).WithMessage("Price cannot exceed 1,000,000");
        
        // Email validation
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();
        
        // Custom validation
        RuleFor(x => x.Category)
            .Must(BeValidCategory).WithMessage("Invalid category");
        
        // Conditional validation
        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.EndDate.HasValue);
    }
    
    private bool BeValidCategory(string category)
    {
        // Custom validation logic
        return !string.IsNullOrEmpty(category);
    }
}
```

---

## 🚫 Common Anti-Patterns to AVOID

### ❌ DON'T DO THIS

```csharp
// ❌ DON'T: Module referencing another module implementation
using FSH.Modules.Identity; // Only use .Contracts

// ❌ DON'T: Putting business logic in endpoints
endpoints.MapPost("/products", (CreateProductCommand cmd) => {
    var product = new Product { Name = cmd.Name }; // Business logic here
    db.Products.Add(product);
    db.SaveChanges();
});

// ❌ DON'T: Using MediatR (use Mediator library)
using MediatR; // Wrong library

// ❌ DON'T: Forgetting multi-tenancy
public class Product // Missing IMustHaveTenant
{
    public Guid Id { get; set; }
}

// ❌ DON'T: Using [ApiController] attribute
[ApiController] // We use Minimal APIs, not controllers

// ❌ DON'T: Synchronous database calls
var users = _db.Users.ToList(); // Use ToListAsync(ct)

// ❌ DON'T: Ignoring CancellationToken
public async Task Handle(Command cmd) // Missing CancellationToken parameter
```

### ✅ DO THIS INSTEAD

```csharp
// ✅ DO: Reference only .Contracts projects
using FSH.Modules.Identity.Contracts.v1.Users;

// ✅ DO: Business logic in handlers
endpoints.MapPost("/products", async (cmd, mediator, ct) => 
    await mediator.Send(cmd, ct));

// ✅ DO: Use Mediator library
using Mediator;

// ✅ DO: Include multi-tenancy
public class Product : IMustHaveTenant
{
    public Guid Id { get; set; }
    public string TenantId { get; set; } = default!;
}

// ✅ DO: Use Minimal APIs with static extension methods
public static RouteHandlerBuilder MapCreateProductEndpoint(...)

// ✅ DO: Use async methods with CancellationToken
var users = await _db.Users.ToListAsync(ct);

// ✅ DO: Always accept CancellationToken
public async ValueTask<Result> Handle(Command cmd, CancellationToken ct)
```

---

## 🧪 Testing Guidelines

### Architecture Tests

The codebase includes architecture tests that enforce these rules:

1. **Module Isolation**: Modules MUST NOT reference other module implementations (only .Contracts)
2. **Version Isolation**: v1 features MUST NOT depend on v2+ features

Run architecture tests:
```bash
dotnet test src/Tests/Architecture.Tests
```

---

## 📚 Additional Resources

### Reference Documents

- `ARCHITECTURE_GUIDE.md` - Comprehensive architecture documentation
- `MODULE_TEMPLATES.md` - Decision trees and templates for different module types
- `CLAUDE.md` - Build & run commands, quick reference
- `QUICKSTART.md` - Getting started guide
- `DEVELOPMENT.md` - Development workflow

### Common Scenarios

| Scenario | Template | Time |
|----------|----------|------|
| Simple CRUD | Template A in MODULE_TEMPLATES.md | 2-3h |
| Workflow/State Machine | Template B in MODULE_TEMPLATES.md | 4-6h |
| File Upload | Template C in MODULE_TEMPLATES.md | 3-4h |
| External API Integration | Template D in MODULE_TEMPLATES.md | 4-5h |
| Cached Read-Heavy | Template E in MODULE_TEMPLATES.md | 3-4h |
| Reporting/Analytics | Template F in MODULE_TEMPLATES.md | 4-6h |
| Background Jobs | Template G in MODULE_TEMPLATES.md | 2-3h |

---

## 🔄 Workflow for Changes

### Before Making Changes

1. ✅ Read this document (COPILOT_INSTRUCTIONS.md)
2. ✅ Review ARCHITECTURE_GUIDE.md for context
3. ✅ Check MODULE_TEMPLATES.md for similar patterns
4. ✅ Identify which module/feature you're modifying

### While Making Changes

1. ✅ Follow the directory structure exactly
2. ✅ Use correct naming conventions
3. ✅ Implement all 4 parts: Command/Query, Validator, Handler, Endpoint
4. ✅ Add multi-tenancy support (IMustHaveTenant)
5. ✅ Include proper validation (FluentValidation)
6. ✅ Use async/await with CancellationToken

### After Making Changes

1. ✅ Build the solution: `dotnet build src/FSH.Framework.slnx`
2. ✅ Run architecture tests: `dotnet test src/Tests/Architecture.Tests`
3. ✅ Run affected feature tests (if any)
4. ✅ Test manually via API or Blazor UI
5. ✅ Update documentation if adding new features

---

## 🎓 Learning Path

### For New Contributors

1. **Day 1**: Read CLAUDE.md + QUICKSTART.md
2. **Day 2**: Read this document (COPILOT_INSTRUCTIONS.md)
3. **Day 3**: Study existing modules (Identity, Multitenancy, Auditing)
4. **Day 4**: Review ARCHITECTURE_GUIDE.md for deep dive
5. **Day 5**: Try implementing a simple CRUD feature using Template A

### For Experienced Contributors

1. Review this document for patterns
2. Check MODULE_TEMPLATES.md for complex scenarios
3. Refer to specific sections as needed

---

## 📞 Support

If you're unsure about:
- **Architecture decisions**: Check ARCHITECTURE_GUIDE.md
- **Implementation patterns**: Check MODULE_TEMPLATES.md
- **Build/run commands**: Check CLAUDE.md
- **Getting started**: Check QUICKSTART.md

---

## 🔍 Summary

**Golden Rules**:
1. Always use Vertical Slice Architecture
2. Always implement CQRS (Command/Query separation)
3. Always include multi-tenancy (IMustHaveTenant)
4. Always use Mediator library (not MediatR)
5. Always validate with FluentValidation
6. Always use async/await with CancellationToken
7. Always follow naming conventions
8. Never reference other module implementations (only .Contracts)
9. Never put business logic in endpoints
10. Never skip architecture tests

**When in doubt, look at existing code in the Identity module for reference!**
