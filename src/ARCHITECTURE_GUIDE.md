# FSH Framework - Complete Architecture & Expansion Guide

## Table of Contents
1. [Architecture Overview](#architecture-overview)
2. [Technology Stack](#technology-stack)
3. [Project Structure](#project-structure)
4. [Core Concepts](#core-concepts)
5. [How to Add New Features](#how-to-add-new-features)
6. [Scaling Strategy](#scaling-strategy)
7. [Best Practices](#best-practices)

---

## Architecture Overview

### **High-Level Architecture**

This is a **Modular Monolith** architecture using **.NET 9** with the following characteristics:

```
┌─────────────────────────────────────────────────────────────────┐
│                    FSH FRAMEWORK ARCHITECTURE                    │
├─────────────────────────────────────────────────────────────────┤
│                                                                  │
│  ┌────────────────────────────────────────────────────────┐    │
│  │           PLAYGROUND (Host Application)                │    │
│  │  ┌──────────────┐         ┌──────────────────────┐    │    │
│  │  │ API          │         │ Blazor Web UI        │    │    │
│  │  │ (Backend)    │◄────────│ (Frontend SSR+WASM)  │    │    │
│  │  └──────────────┘         └──────────────────────┘    │    │
│  └────────────────────────────────────────────────────────┘    │
│                           │                                      │
│                           │ Uses                                 │
│                           ▼                                      │
│  ┌────────────────────────────────────────────────────────┐    │
│  │                  MODULES (Business Logic)              │    │
│  │  ┌──────────┐  ┌──────────────┐  ┌───────────────┐   │    │
│  │  │ Identity │  │ Multitenancy │  │ Auditing      │   │    │
│  │  │ Module   │  │ Module       │  │ Module        │   │    │
│  │  └──────────┘  └──────────────┘  └───────────────┘   │    │
│  │                                                         │    │
│  │  Each module has:                                      │    │
│  │  • Domain Models                                       │    │
│  │  • Features (CQRS)                                     │    │
│  │  • Data Context                                        │    │
│  │  • Events                                              │    │
│  │  • API Endpoints                                       │    │
│  └────────────────────────────────────────────────────────┘    │
│                           │                                      │
│                           │ Built on                             │
│                           ▼                                      │
│  ┌────────────────────────────────────────────────────────┐    │
│  │            BUILDING BLOCKS (Infrastructure)            │    │
│  │  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────────┐     │    │
│  │  │ Core   │ │ Web    │ │ Caching│ │ Persistence│     │    │
│  │  └────────┘ └────────┘ └────────┘ └────────────┘     │    │
│  │  ┌────────┐ ┌────────┐ ┌────────┐ ┌────────────┐     │    │
│  │  │Eventing│ │ Jobs   │ │ Storage│ │  Mailing   │     │    │
│  │  └────────┘ └────────┘ └────────┘ └────────────┘     │    │
│  │  ┌────────┐ ┌────────┐                               │    │
│  │  │Blazor  │ │ Shared │                               │    │
│  │  │  UI    │ │        │                               │    │
│  │  └────────┘ └────────┘                               │    │
│  └────────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────────┘
```

### **Key Architecture Patterns**

1. **Modular Monolith**: Separate modules with clear boundaries, can be extracted to microservices later
2. **CQRS (Command Query Responsibility Segregation)**: Separate read and write operations
3. **Vertical Slice Architecture**: Features organized by business capability
4. **Repository Pattern**: Data access abstraction
5. **Mediator Pattern**: Decoupled request/response handling (using `Mediator` library)
6. **Event-Driven**: Integration events for cross-module communication
7. **Multi-tenancy**: Full tenant isolation at database level

---

## Technology Stack

### **Backend (API)**
- **.NET 9.0** - Latest framework
- **ASP.NET Core Minimal APIs** - Fast, lightweight endpoints
- **Entity Framework Core** - ORM for data access
- **PostgreSQL** - Primary database (supports multiple providers)
- **Mediator** - CQRS implementation
- **FluentValidation** - Input validation
- **Hangfire** - Background job processing
- **StackExchange.Redis** - Distributed caching
- **Finbuckle.MultiTenant** - Multi-tenancy framework
- **OpenTelemetry** - Observability (metrics, tracing, logging)
- **Serilog** - Structured logging
- **Swagger/Scalar** - API documentation
- **AspNetCore.HealthChecks** - Health monitoring

### **Frontend (Blazor)**
- **Blazor Web** (.NET 9) - SSR (Server-Side Rendering) + WebAssembly
- **MudBlazor** - UI component library
- **NSwag** - Auto-generated API clients
- **Cookie Authentication** - SSR-compatible auth
- **Response Compression** - Brotli/Gzip
- **Output Caching** - Performance optimization

### **Infrastructure**
- **Docker** - Containerization (via .NET Aspire)
- **.NET Aspire** - Cloud-native orchestration
- **Redis** - Distributed cache
- **SMTP** - Email delivery

---

## Project Structure

### **Directory Layout**

```
/src
├── BuildingBlocks/          # Reusable infrastructure components
│   ├── Blazor.UI/          # Blazor UI components & services
│   ├── Caching/            # Redis/in-memory caching
│   ├── Core/               # Core abstractions & domain primitives
│   ├── Eventing/           # Event bus (Inbox/Outbox pattern)
│   ├── Jobs/               # Hangfire background jobs
│   ├── Mailing/            # Email services
│   ├── Persistence/        # EF Core extensions & base context
│   ├── Shared/             # Shared DTOs, constants, interfaces
│   ├── Storage/            # File storage (local/S3)
│   └── Web/                # Web infrastructure (auth, CORS, OpenAPI, etc.)
│
├── Modules/                 # Business domain modules
│   ├── Auditing/           # Audit trail module
│   │   ├── Modules.Auditing/          # Implementation
│   │   └── Modules.Auditing.Contracts/ # Public contracts
│   ├── Identity/           # User & authentication module
│   │   ├── Modules.Identity/          # Implementation
│   │   └── Modules.Identity.Contracts/ # Public contracts
│   └── Multitenancy/       # Tenant management module
│       ├── Modules.Multitenancy/          # Implementation
│       └── Modules.Multitenancy.Contracts/ # Public contracts
│
├── Playground/             # Host applications
│   ├── Playground.Api/     # Backend API host
│   ├── Playground.Blazor/  # Frontend Blazor host
│   ├── FSH.Playground.AppHost/ # .NET Aspire orchestrator
│   └── Migrations.PostgreSQL/  # Database migrations
│
├── Tests/                  # Test projects
│   ├── Architecture.Tests/ # Architecture rules tests
│   └── Multitenacy.Tests/ # Multi-tenancy tests
│
└── Tools/                  # Developer tools
    └── CLI/                # Command-line tools

```

---

## Core Concepts

### **1. Module Structure**

Each module follows this structure:

```
Modules.{ModuleName}/
├── {ModuleName}Module.cs        # Module registration (IModule)
├── Domain/                      # Domain entities
├── Data/                        # EF Core DbContext
│   ├── {ModuleName}DbContext.cs
│   └── Configurations/          # Entity configurations
├── Features/                    # Vertical slices
│   └── v1/                      # API version
│       └── {FeatureName}/
│           ├── {Feature}Command.cs       # Command/Query (in Contracts)
│           ├── {Feature}CommandHandler.cs # Handler
│           ├── {Feature}CommandValidator.cs # Validation
│           └── {Feature}Endpoint.cs       # Minimal API endpoint
├── Events/                      # Domain/Integration events
├── Services/                    # Domain services
└── Authorization/               # Permissions & policies

Modules.{ModuleName}.Contracts/  # Public API
├── v1/                          # Versioned contracts
│   └── {Feature}/
│       ├── {Feature}Command.cs  # Request DTO
│       └── {Feature}Response.cs # Response DTO
└── DTOs/                        # Shared DTOs
```

### **2. CQRS Pattern**

Every feature uses the CQRS pattern:

**Command (Write Operation):**
```csharp
// In Contracts project
public record CreateUserCommand(string Email, string Name) : ICommand<Guid>;

// In Module project
public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateUserCommand command, CancellationToken ct)
    {
        // 1. Validate
        // 2. Create entity
        // 3. Save to database
        // 4. Publish events
        // 5. Return result
    }
}

// Validator
public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Name).NotEmpty().MaximumLength(100);
    }
}

// Endpoint
public static RouteHandlerBuilder MapCreateUserEndpoint(this IEndpointRouteBuilder endpoint)
{
    return endpoint.MapPost("/users",
        async (CreateUserCommand command, IMediator mediator, CancellationToken ct) =>
        {
            var userId = await mediator.Send(command, ct);
            return TypedResults.Created($"/users/{userId}", userId);
        })
        .WithName("CreateUser")
        .WithSummary("Create a new user")
        .RequireAuthorization("users:create");
}
```

**Query (Read Operation):**
```csharp
// In Contracts
public record GetUserQuery(Guid UserId) : IQuery<UserResponse>;

// In Module
public class GetUserQueryHandler : IQueryHandler<GetUserQuery, UserResponse>
{
    private readonly IdentityDbContext _db;
    
    public async ValueTask<UserResponse> Handle(GetUserQuery query, CancellationToken ct)
    {
        var user = await _db.Users.FindAsync(query.UserId, ct);
        return user.ToResponse();
    }
}
```

### **3. Multi-Tenancy**

The framework supports **database-per-tenant** architecture:

```csharp
// Tenant resolution happens automatically via:
// 1. HTTP Header: "tenant: {tenant-identifier}"
// 2. JWT Claim: "tenant"
// 3. Query Parameter: "?tenant={tenant-identifier}"

// Each tenant gets:
// - Separate database connection
// - Isolated data
// - Custom theme/branding
// - Independent migrations
```

**Adding Tenant-Aware Entity:**
```csharp
public class Product : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TenantId { get; set; } // Required for multi-tenancy
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
```

### **4. Event-Driven Communication**

**Outbox Pattern (Reliable event publishing):**
```csharp
// Publish integration event
public class UserCreatedEventHandler : INotificationHandler<UserCreatedEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public async Task Handle(UserCreatedEvent notification, CancellationToken ct)
    {
        await _eventPublisher.PublishAsync(
            new UserCreatedIntegrationEvent(notification.UserId, notification.Email), 
            ct);
    }
}
```

**Inbox Pattern (Idempotent event consumption):**
```csharp
public class UserCreatedIntegrationEventHandler 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task HandleAsync(UserCreatedIntegrationEvent @event, CancellationToken ct)
    {
        // Process event (guaranteed once)
    }
}
```

### **5. API Versioning**

Features are organized by version:

```
Features/
├── v1/          # Version 1.0
│   ├── Users/
│   └── Roles/
└── v2/          # Version 2.0 (future)
    └── Users/
```

Endpoints use API versioning:
```csharp
var versionSet = endpoints.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .Build();

endpoint.MapPost("/users", handler)
    .WithApiVersionSet(versionSet)
    .MapToApiVersion(1);
```

---

## How to Add New Features

### **Step-by-Step Guide: Adding a New "Products" Module**

#### **Step 1: Create Module Structure**

```bash
# Create directory structure
mkdir -p Modules/Products/Modules.Products
mkdir -p Modules/Products/Modules.Products.Contracts
```

#### **Step 2: Create Contracts Project**

**File: `Modules/Products/Modules.Products.Contracts/Modules.Products.Contracts.csproj`**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Mediator.Abstractions" />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="../../BuildingBlocks/Shared/Shared.csproj" />
  </ItemGroup>
</Project>
```

**File: `Modules/Products/Modules.Products.Contracts/v1/Products/CreateProductCommand.cs`**
```csharp
using Mediator;

namespace FSH.Modules.Products.Contracts.v1.Products;

public record CreateProductCommand(
    string Name,
    string Description,
    decimal Price,
    string Category
) : ICommand<Guid>;
```

**File: `Modules/Products/Modules.Products.Contracts/v1/Products/ProductResponse.cs`**
```csharp
namespace FSH.Modules.Products.Contracts.v1.Products;

public record ProductResponse(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    string Category,
    DateTimeOffset CreatedOn
);
```

#### **Step 3: Create Module Implementation Project**

**File: `Modules/Products/Modules.Products/Modules.Products.csproj`**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <TargetFramework>net9.0</TargetFramework>
  </PropertyGroup>
  
  <ItemGroup>
    <PackageReference Include="Mediator" />
    <PackageReference Include="FluentValidation" />
  </ItemGroup>
  
  <ItemGroup>
    <ProjectReference Include="../../BuildingBlocks/Web/Web.csproj" />
    <ProjectReference Include="../../BuildingBlocks/Persistence/Persistence.csproj" />
    <ProjectReference Include="../Modules.Products.Contracts/Modules.Products.Contracts.csproj" />
  </ItemGroup>
</Project>
```

#### **Step 4: Create Domain Entity**

**File: `Modules/Products/Modules.Products/Domain/Product.cs`**
```csharp
using FSH.Framework.Shared.Persistence;

namespace FSH.Modules.Products.Domain;

public class Product : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Description { get; set; } = default!;
    public decimal Price { get; set; }
    public string Category { get; set; } = default!;
    
    // Multi-tenancy
    public string TenantId { get; set; } = default!;
    
    // Audit fields
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
```

#### **Step 5: Create DbContext**

**File: `Modules/Products/Modules.Products/Data/ProductsDbContext.cs`**
```csharp
using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Eventing.Inbox;
using FSH.Framework.Eventing.Outbox;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Modules.Products.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FSH.Modules.Products.Data;

public class ProductsDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;
    
    public DbSet<Product> Products => Set<Product>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    
    public ProductsDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<ProductsDbContext> options,
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
        builder.ApplyConfigurationsFromAssembly(typeof(ProductsDbContext).Assembly);
        
        builder.ApplyConfiguration(new OutboxMessageConfiguration("products"));
        builder.ApplyConfiguration(new InboxMessageConfiguration("products"));
    }
    
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

**File: `Modules/Products/Modules.Products/Data/Configurations/ProductConfiguration.cs`**
```csharp
using FSH.Modules.Products.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Products.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "products");
        
        builder.HasKey(x => x.Id);
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);
        
        builder.Property(x => x.Description)
            .HasMaxLength(1000);
        
        builder.Property(x => x.Price)
            .HasColumnType("decimal(18,2)");
        
        builder.Property(x => x.Category)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Category);
    }
}
```

#### **Step 6: Create Feature (Command/Handler/Validator/Endpoint)**

**File: `Modules/Products/Modules.Products/Features/v1/Products/CreateProduct/CreateProductCommandHandler.cs`**
```csharp
using FSH.Framework.Core.Context;
using FSH.Modules.Products.Contracts.v1.Products;
using FSH.Modules.Products.Data;
using FSH.Modules.Products.Domain;
using Mediator;

namespace FSH.Modules.Products.Features.v1.Products.CreateProduct;

public class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Guid>
{
    private the ProductsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public CreateProductCommandHandler(ProductsDbContext db, ICurrentUser currentUser)
    {
        _db = db;
        _currentUser = currentUser;
    }
    
    public async ValueTask<Guid> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description,
            Price = command.Price,
            Category = command.Category,
            TenantId = _currentUser.TenantId!,
            CreatedOn = DateTimeOffset.UtcNow,
            CreatedBy = _currentUser.UserId
        };
        
        _db.Products.Add(product);
        await _db.SaveChangesAsync(ct);
        
        return product.Id;
    }
}
```

**File: `Modules/Products/Modules.Products/Features/v1/Products/CreateProduct/CreateProductCommandValidator.cs`**
```csharp
using FluentValidation;
using FSH.Modules.Products.Contracts.v1.Products;

namespace FSH.Modules.Products.Features.v1.Products.CreateProduct;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);
        
        RuleFor(x => x.Description)
            .MaximumLength(1000);
        
        RuleFor(x => x.Price)
            .GreaterThan(0);
        
        RuleFor(x => x.Category)
            .NotEmpty()
            .MaximumLength(100);
    }
}
```

**File: `Modules/Products/Modules.Products/Features/v1/Products/CreateProduct/CreateProductEndpoint.cs`**
```csharp
using FSH.Modules.Products.Contracts.v1.Products;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Products.Features.v1.Products.CreateProduct;

public static class CreateProductEndpoint
{
    public static RouteHandlerBuilder MapCreateProductEndpoint(this IEndpointRouteBuilder endpoint)
    {
        return endpoint.MapPost("/products",
            async Task<Results<Created<Guid>, ValidationProblem>> 
            (CreateProductCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var productId = await mediator.Send(command, ct);
                return TypedResults.Created($"/products/{productId}", productId);
            })
            .WithName("CreateProduct")
            .WithSummary("Create a new product")
            .WithDescription("Creates a new product in the catalog")
            .WithTags("Products")
            .RequireAuthorization("products:create")
            .Produces<Guid>(StatusCodes.Status201Created)
            .ProducesValidationProblem();
    }
}
```

#### **Step 7: Create Module Registration**

**File: `Modules/Products/Modules.Products/ProductsModule.cs`**
```csharp
using Asp.Versioning;
using FSH.Framework.Persistence;
using FSH.Framework.Web.Modules;
using FSH.Modules.Products.Data;
using FSH.Modules.Products.Features.v1.Products.CreateProduct;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.Products;

public class ProductsModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register DbContext
        builder.Services.AddHeroDbContext<ProductsDbContext>();
        
        // Register eventing
        builder.Services.AddEventingCore(builder.Configuration);
        builder.Services.AddEventingForDbContext<ProductsDbContext>();
        
        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<ProductsDbContext>(
                name: "db:products",
                failureStatus: HealthStatus.Unhealthy);
    }
    
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();
        
        var group = endpoints.MapGroup("/api/v{version:apiVersion}/products")
            .WithApiVersionSet(versionSet)
            .WithTags("Products");
        
        // Map endpoints
        group.MapCreateProductEndpoint();
        // Add more endpoints here...
    }
}
```

#### **Step 8: Register Module in Host Application**

**File: `Playground/Playground.Api/Program.cs`**
```csharp
// Add to using statements
using FSH.Modules.Products;

// Add to module assemblies array
var moduleAssemblies = new Assembly[]
{
    typeof(IdentityModule).Assembly,
    typeof(MultitenancyModule).Assembly,
    typeof(AuditingModule).Assembly,
    typeof(ProductsModule).Assembly  // ← Add this
};

// Add to Mediator assemblies
builder.Services.AddMediator(o =>
{
    o.ServiceLifetime = ServiceLifetime.Scoped;
    o.Assemblies = [
        // ... existing assemblies ...
        typeof(CreateProductCommand),  // ← Add contracts
        typeof(CreateProductCommandHandler)  // ← Add handlers
    ];
});
```

#### **Step 9: Add Database Migration**

```bash
# Navigate to Migrations project
cd Playground/Migrations.PostgreSQL

# Add migration
dotnet ef migrations add AddProductsModule \
    --context ProductsDbContext \
    --startup-project ../Playground.Api

# Update database
dotnet ef database update \
    --context ProductsDbContext \
    --startup-project ../Playground.Api
```

#### **Step 10: Add Blazor UI (Optional)**

**File: `Playground/Playground.Blazor/Components/Pages/Products/ProductsList.razor`**
```razor
@page "/products"
@attribute [Authorize(Policy = "products:read")]
@inject IProductsClient ProductsClient

<PageTitle>Products</PageTitle>

<MudText Typo="Typo.h4" GutterBottom="true">Products</MudText>

<MudTable Items="@_products" Loading="@_loading">
    <HeaderContent>
        <MudTh>Name</MudTh>
        <MudTh>Category</MudTh>
        <MudTh>Price</MudTh>
        <MudTh>Actions</MudTh>
    </HeaderContent>
    <RowTemplate>
        <MudTd>@context.Name</MudTd>
        <MudTd>@context.Category</MudTd>
        <MudTd>$@context.Price.ToString("F2")</MudTd>
        <MudTd>
            <MudIconButton Icon="@Icons.Material.Filled.Edit" 
                          OnClick="() => EditProduct(context.Id)" />
            <MudIconButton Icon="@Icons.Material.Filled.Delete" 
                          OnClick="() => DeleteProduct(context.Id)" />
        </MudTd>
    </RowTemplate>
</MudTable>

@code {
    private List<ProductResponse> _products = new();
    private bool _loading = true;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _products = await ProductsClient.GetProductsAsync();
        }
        finally
        {
            _loading = false;
        }
    }
}
```

#### **Step 11: Generate API Client**

Update NSwag configuration and regenerate:

```bash
# Add to nswag.json in Playground.Blazor
{
  "operationGenerationMode": "MultipleClientsFromOperationId",
  "className": "ProductsClient",
  "generateClientInterfaces": true
}

# Regenerate clients
make nswag
```

---

## Scaling Strategy

### **Phase 1: Modular Monolith (Current State)**
✅ **You are here**

**Characteristics:**
- Single deployment unit
- Shared database (tenant-isolated)
- Fast development
- Easy debugging
- Vertical scaling

**When to use:**
- MVP/Early stage
- Small to medium teams (1-10 developers)
- < 100K requests/day

### **Phase 2: Distributed Monolith**
**Split by deployment, not codebase**

```
┌────────────────┐     ┌────────────────┐     ┌────────────────┐
│  Identity API  │     │ Products API   │     │  Orders API    │
│  (Module)      │────▶│  (Module)      │────▶│  (Module)      │
└────────────────┘     └────────────────┘     └────────────────┘
        │                      │                      │
        └──────────────────────┴──────────────────────┘
                               │
                       ┌───────▼───────┐
                       │  Shared DB    │
                       │  (per tenant) │
                       └───────────────┘
```

**Changes needed:**
1. Deploy modules as separate services
2. Add API Gateway (e.g., YARP, Ocelot)
3. Use distributed tracing (already have OpenTelemetry)
4. Add service discovery (Consul, Eureka)

**When to use:**
- Multiple teams
- Different scaling requirements per module
- 100K - 1M requests/day

### **Phase 3: Microservices**
**Full autonomy per service**

```
┌─────────────────┐   ┌─────────────────┐   ┌─────────────────┐
│  Identity       │   │  Products       │   │  Orders         │
│  Service        │   │  Service        │   │  Service        │
│  ┌───────────┐  │   │  ┌───────────┐  │   │  ┌───────────┐  │
│  │ Identity  │  │   │  │ Products  │  │   │  │ Orders    │  │
│  │ DB        │  │   │  │ DB        │  │   │  │ DB        │  │
│  └───────────┘  │   │  └───────────┘  │   │  └───────────┘  │
└─────────────────┘   └─────────────────┘   └─────────────────┘
```

**Changes needed:**
1. Separate database per service
2. Implement Saga pattern for distributed transactions
3. Add message broker (RabbitMQ, Kafka)
4. Implement API versioning strictly
5. Add circuit breakers (Polly)
6. Service mesh (Istio, Linkerd) for advanced scenarios

**When to use:**
- Large organization (50+ developers)
- Need to scale services independently
- > 1M requests/day
- Different technology stacks per service

### **Phase 4: Serverless/Cloud-Native**

Move compute-intensive or event-driven workloads to serverless:

```
┌─────────────┐
│ Azure       │
│ Functions   │◄───── Event Grid ◄───── Service Bus
│ (C#)        │
└─────────────┘

┌─────────────┐
│ AWS Lambda  │◄───── SNS/SQS
│ (C#)        │
└─────────────┘
```

**When to use:**
- Unpredictable traffic
- Event-driven workflows
- Cost optimization for sporadic workloads

---

## Best Practices

### **1. Module Design Principles**

✅ **DO:**
- Keep modules loosely coupled
- Use integration events for cross-module communication
- Define clear contracts in `.Contracts` projects
- One DbContext per module
- Follow domain-driven design (DDD) principles

❌ **DON'T:**
- Reference other module implementations directly
- Share database tables between modules
- Put business logic in endpoints
- Use static/global state

### **2. API Design**

✅ **DO:**
- Use Minimal APIs for simplicity
- Version your APIs (`/api/v1`, `/api/v2`)
- Use typed results (`Results<Ok<T>, NotFound>`)
- Document with OpenAPI/Swagger
- Use proper HTTP status codes

❌ **DON'T:**
- Return bare exceptions to clients
- Use `[ApiController]` attribute (use Minimal APIs)
- Expose internal IDs or implementation details

### **3. Database**

✅ **DO:**
- Use migrations for schema changes
- Add indexes for query performance
- Use soft deletes for auditing
- Implement proper foreign keys
- Use connection pooling

❌ **DON'T:**
- Use raw SQL unless necessary
- Store sensitive data unencrypted
- Forget to add multi-tenancy filters

### **4. Security**

✅ **DO:**
- Use JWT tokens with short expiry
- Implement refresh tokens
- Use HTTPS everywhere
- Validate all inputs
- Implement rate limiting
- Use parameterized queries

❌ **DON'T:**
- Store passwords in plain text
- Trust client-side validation alone
- Expose stack traces in production

### **5. Performance**

✅ **DO:**
- Use caching (Redis) for read-heavy data
- Implement pagination for large datasets
- Use async/await consistently
- Enable response compression
- Use EF Core query splitting for large results

❌ **DON'T:**
- Load entire tables into memory
- Use synchronous I/O operations
- Forget to dispose resources

---

## Where to Put Things

### **Adding a New Custom API Endpoint**

**Location:** `Modules/{ModuleName}/Features/v1/{FeatureName}/{Action}Endpoint.cs`

**Example:** `Modules/Products/Features/v1/Products/SearchProducts/SearchProductsEndpoint.cs`

### **Adding a New UI Page**

**Location:** `Playground/Playground.Blazor/Components/Pages/{Feature}/{PageName}.razor`

**Example:** `Playground/Playground.Blazor/Components/Pages/Products/ProductsList.razor`

### **Adding a Shared Component**

**Location:** `BuildingBlocks/Blazor.UI/Components/{ComponentName}.razor`

**Example:** `BuildingBlocks/Blazor.UI/Components/DataGrid.razor`

### **Adding a Background Job**

**Location:** `Modules/{ModuleName}/Jobs/{JobName}Job.cs`

**Example:**
```csharp
using Hangfire;

namespace FSH.Modules.Products.Jobs;

public class ProductPriceUpdateJob
{
    private readonly ProductsDbContext _db;
    
    public ProductPriceUpdateJob(ProductsDbContext db)
    {
        _db = db;
    }
    
    [AutomaticRetry(Attempts = 3)]
    public async Task ExecuteAsync(CancellationToken ct)
    {
        // Job logic here
    }
}

// Register in ProductsModule.cs
RecurringJob.AddOrUpdate<ProductPriceUpdateJob>(
    "update-product-prices",
    job => job.ExecuteAsync(CancellationToken.None),
    Cron.Daily);
```

### **Adding Integration Event Handlers**

**Location:** `Modules/{ModuleName}/Events/{EventName}Handler.cs`

**Example:**
```csharp
using FSH.Framework.Eventing;
using FSH.Modules.Identity.Contracts.Events;

namespace FSH.Modules.Products.Events;

public class UserCreatedEventHandler 
    : IIntegrationEventHandler<UserCreatedIntegrationEvent>
{
    public async Task HandleAsync(
        UserCreatedIntegrationEvent @event, 
        CancellationToken ct)
    {
        // Handle the event
    }
}
```

### **Adding Custom Middleware**

**Location:** `BuildingBlocks/Web/Middleware/{MiddlewareName}.cs`

**Example:**
```csharp
public class CustomMiddleware
{
    private readonly RequestDelegate _next;
    
    public CustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        // Before request
        await _next(context);
        // After request
    }
}

// Register in Extensions.cs
app.UseMiddleware<CustomMiddleware>();
```

---

## Quick Reference Commands

```bash
# Run API
make api-http

# Run Blazor UI
make blazor-http

# Run with Aspire (Redis + PostgreSQL)
make apphost

# Generate API clients
make nswag

# Kill all services
make kill-ports

# Add migration
cd Playground/Migrations.PostgreSQL
dotnet ef migrations add MigrationName \
    --context {YourContext} \
    --startup-project ../Playground.Api

# Run tests
dotnet test

# Build solution
dotnet build

# Restore packages
dotnet restore
```

---

## Summary

This FSH Framework is a **production-ready**, **scalable**, **modular monolith** built on **.NET 9** that can evolve from a simple API to a distributed microservices architecture.

### **Key Strengths:**
✅ **Modular Architecture** - Clear separation of concerns
✅ **Multi-Tenancy** - Database-per-tenant isolation
✅ **CQRS + Mediator** - Clean, testable code
✅ **Event-Driven** - Loosely coupled modules
✅ **Blazor UI** - Modern, fast frontend
✅ **Enterprise Features** - Auth, auditing, caching, jobs
✅ **Scalability Path** - Can grow to microservices

### **Best For:**
- SaaS applications
- Multi-tenant B2B platforms
- Enterprise applications
- MVP to production evolution

### **Next Steps:**
1. ✅ Read this guide thoroughly
2. ✅ Create your first module following the Products example
3. ✅ Understand the CQRS pattern
4. ✅ Practice adding features
5. ✅ Explore the existing modules (Identity, Multitenancy, Auditing)

**You're ready to build!** 🚀

