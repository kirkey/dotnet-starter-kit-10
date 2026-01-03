# FSH Framework - Quick Reference Card

## 📁 Project Structure At-A-Glance

```
/src
├── 🧱 BuildingBlocks/          Infrastructure (reusable)
├── 📦 Modules/                 Business logic (your features)
├── 🎮 Apps/              Host applications (API + Blazor)
├── 🧪 Tests/                   Test projects
└── 🛠️ Tools/                   CLI utilities
```

---

## 🚀 Quick Start Commands

```bash
# Setup HTTPS certificate (first time)
make setup

# Run API + Blazor (2 terminals)
make api-http      # Terminal 1
make blazor-http   # Terminal 2

# Full stack with Docker
make apphost

# Generate API clients
make nswag

# Stop everything
make kill-ports

# Show all commands
make help
```

---

## 📦 Adding a New Module (5-Minute Checklist)

### 1️⃣ Create Projects
```bash
mkdir -p Modules/YourModule/Modules.YourModule
mkdir -p Modules/YourModule/Modules.YourModule.Contracts
```

### 2️⃣ Create `.csproj` Files
**Contracts:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <PackageReference Include="Mediator.Abstractions" />
    <ProjectReference Include="../../BuildingBlocks/Shared/Shared.csproj" />
  </ItemGroup>
</Project>
```

**Implementation:**
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <PackageReference Include="Mediator" />
    <PackageReference Include="FluentValidation" />
    <ProjectReference Include="../../BuildingBlocks/Web/Web.csproj" />
    <ProjectReference Include="../../BuildingBlocks/Persistence/Persistence.csproj" />
    <ProjectReference Include="../Modules.YourModule.Contracts/Modules.YourModule.Contracts.csproj" />
  </ItemGroup>
</Project>
```

### 3️⃣ Create Domain Entity
```csharp
// Modules/YourModule/Modules.YourModule/Domain/YourEntity.cs
public class YourEntity : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TenantId { get; set; } // Multi-tenancy
    
    // Audit fields (auto-populated)
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
```

### 4️⃣ Create DbContext
```csharp
// Modules/YourModule/Modules.YourModule/Data/YourModuleDbContext.cs
public class YourModuleDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private AppTenantInfo TenantInfo { get; set; }
    
    public DbSet<YourEntity> YourEntities => Set<YourEntity>();
    public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    public DbSet<InboxMessage> InboxMessages => Set<InboxMessage>();
    
    public YourModuleDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        DbContextOptions<YourModuleDbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment) : base(options)
    {
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(YourModuleDbContext).Assembly);
        builder.ApplyConfiguration(new OutboxMessageConfiguration("yourmodule"));
        builder.ApplyConfiguration(new InboxMessageConfiguration("yourmodule"));
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.ConfigureHeroDatabase(
                _settings.Provider,
                TenantInfo.ConnectionString,
                _settings.MigrationsAssembly,
                Environment.IsDevelopment());
        }
    }
}
```

### 5️⃣ Create Feature (CQRS)

**Command (in Contracts):**
```csharp
// Modules.YourModule.Contracts/v1/CreateYourEntityCommand.cs
public record CreateYourEntityCommand(string Name) : ICommand<Guid>;
```

**Handler:**
```csharp
// Modules.YourModule/Features/v1/CreateYourEntity/CreateYourEntityCommandHandler.cs
public class CreateYourEntityCommandHandler : ICommandHandler<CreateYourEntityCommand, Guid>
{
    private readonly YourModuleDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<Guid> Handle(CreateYourEntityCommand command, CancellationToken ct)
    {
        var entity = new YourEntity
        {
            Id = Guid.NewGuid(),
            Name = command.Name,
            TenantId = _currentUser.TenantId!
        };
        
        _db.YourEntities.Add(entity);
        await _db.SaveChangesAsync(ct);
        
        return entity.Id;
    }
}
```

**Validator:**
```csharp
// Modules.YourModule/Features/v1/CreateYourEntity/CreateYourEntityCommandValidator.cs
public class CreateYourEntityCommandValidator : AbstractValidator<CreateYourEntityCommand>
{
    public CreateYourEntityCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
    }
}
```

**Endpoint:**
```csharp
// Modules.YourModule/Features/v1/CreateYourEntity/CreateYourEntityEndpoint.cs
public static class CreateYourEntityEndpoint
{
    public static RouteHandlerBuilder MapCreateYourEntityEndpoint(this IEndpointRouteBuilder endpoint)
    {
        return endpoint.MapPost("/your-entities",
            async (CreateYourEntityCommand command, IMediator mediator, CancellationToken ct) =>
            {
                var id = await mediator.Send(command, ct);
                return TypedResults.Created($"/your-entities/{id}", id);
            })
            .WithName("CreateYourEntity")
            .RequireAuthorization("your-entities:create");
    }
}
```

### 6️⃣ Create Module Registration
```csharp
// Modules/YourModule/Modules.YourModule/YourModuleModule.cs
public class YourModuleModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        builder.Services.AddHeroDbContext<YourModuleDbContext>();
        builder.Services.AddEventingCore(builder.Configuration);
        builder.Services.AddEventingForDbContext<YourModuleDbContext>();
        
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<YourModuleDbContext>("db:yourmodule");
    }
    
    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        var versionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .Build();
        
        var group = endpoints.MapGroup("/api/v{version:apiVersion}/your-entities")
            .WithApiVersionSet(versionSet)
            .WithTags("YourModule");
        
        group.MapCreateYourEntityEndpoint();
    }
}
```

### 7️⃣ Register in Host (Program.cs)
```csharp
// Add to using statements
using FSH.Modules.YourModule;

// Add to module assemblies
var moduleAssemblies = new Assembly[]
{
    typeof(IdentityModule).Assembly,
    typeof(MultitenancyModule).Assembly,
    typeof(AuditingModule).Assembly,
    typeof(YourModuleModule).Assembly  // ← Add this
};

// Add to Mediator
builder.Services.AddMediator(o =>
{
    o.Assemblies = [
        // ... existing ...
        typeof(CreateYourEntityCommand),
        typeof(CreateYourEntityCommandHandler)
    ];
});
```

### 8️⃣ Create Migration
```bash
cd Apps/Migrations.PostgreSQL
dotnet ef migrations add AddYourModule \
    --context YourModuleDbContext \
    --startup-project ../Apps.Api

dotnet ef database update \
    --context YourModuleDbContext \
    --startup-project ../Apps.Api
```

---

## 🎨 Adding Blazor UI Page

### 1️⃣ Create Page Component
```razor
@* Apps/Apps.Blazor/Components/Pages/YourModule/YourEntityList.razor *@
@page "/your-entities"
@attribute [Authorize(Policy = "your-entities:read")]
@inject IYourModuleClient ApiClient

<PageTitle>Your Entities</PageTitle>

<MudText Typo="Typo.h4">Your Entities</MudText>

<MudTable Items="@_entities" Loading="@_loading">
    <HeaderContent>
        <MudTh>Name</MudTh>
        <MudTh>Created</MudTh>
        <MudTh>Actions</MudTh>
    </HeaderContent>
    <RowTemplate>
        <MudTd>@context.Name</MudTd>
        <MudTd>@context.CreatedOn.ToString("g")</MudTd>
        <MudTd>
            <MudIconButton Icon="@Icons.Material.Filled.Edit" OnClick="() => Edit(context.Id)" />
        </MudTd>
    </RowTemplate>
</MudTable>

@code {
    private List<YourEntityResponse> _entities = new();
    private bool _loading = true;
    
    protected override async Task OnInitializedAsync()
    {
        try
        {
            _entities = await ApiClient.GetYourEntitiesAsync();
        }
        finally
        {
            _loading = false;
        }
    }
}
```

### 2️⃣ Generate API Client
```bash
make nswag
```

### 3️⃣ Add to Navigation
```razor
@* Apps/Apps.Blazor/Components/Layout/NavMenu.razor *@
<MudNavLink Href="/your-entities" Icon="@Icons.Material.Filled.List">
    Your Entities
</MudNavLink>
```

---

## 🔐 Permission Management

### Define Permission
```csharp
// In YourModuleModule.cs or separate class
public static class YourModulePermissions
{
    public const string View = "your-entities:read";
    public const string Create = "your-entities:create";
    public const string Edit = "your-entities:update";
    public const string Delete = "your-entities:delete";
}
```

### Secure Endpoint
```csharp
endpoint.MapPost("/your-entities", handler)
    .RequireAuthorization(YourModulePermissions.Create);
```

### Secure Blazor Page
```razor
@attribute [Authorize(Policy = YourModulePermissions.View)]
```

---

## 🗄️ Common Database Patterns

### Soft Delete
```csharp
public class YourEntity : ISoftDelete
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTimeOffset? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
}

// In DbContext
protected override void OnModelCreating(ModelBuilder builder)
{
    builder.Entity<YourEntity>()
        .HasQueryFilter(e => !e.IsDeleted); // Global filter
}
```

### One-to-Many
```csharp
public class Order
{
    public Guid Id { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Order Order { get; set; }
}

// Configuration
builder.Entity<Order>()
    .HasMany(o => o.Items)
    .WithOne(i => i.Order)
    .HasForeignKey(i => i.OrderId)
    .OnDelete(DeleteBehavior.Cascade);
```

### Many-to-Many
```csharp
public class User
{
    public Guid Id { get; set; }
    public List<Role> Roles { get; set; } = new();
}

public class Role
{
    public Guid Id { get; set; }
    public List<User> Users { get; set; } = new();
}

// Configuration (EF Core 5+)
builder.Entity<User>()
    .HasMany(u => u.Roles)
    .WithMany(r => r.Users);
```

---

## 🔥 Hot Tips

### ✅ Always Use
- `async/await` for I/O operations
- DTOs for API responses (never expose entities)
- Validation (`FluentValidation`)
- `CancellationToken` in handlers
- Multi-tenancy filters (`IMustHaveTenant`)
- Audit fields (`IAuditableEntity`)

### ❌ Never
- Return entities from API endpoints
- Use `.Result` or `.Wait()` on async calls
- Store sensitive data without encryption
- Skip input validation
- Forget to add indexes
- Use `SELECT *` (use projections)

### 🎯 Performance
```csharp
// Use AsNoTracking() for read-only queries
var products = await _db.Products.AsNoTracking().ToListAsync();

// Use projections instead of loading full entities
var products = await _db.Products
    .Select(p => new ProductDto(p.Id, p.Name))
    .ToListAsync();

// Cache frequently accessed data
var settings = await _cache.GetOrSetAsync(
    "settings:tenant:123",
    () => _db.Settings.FindAsync(id),
    TimeSpan.FromHours(1));
```

---

## 📊 Architecture Layers

```
┌─────────────────────────────────────────┐
│         API Layer (Endpoints)           │ ← Minimal APIs
├─────────────────────────────────────────┤
│      Application Layer (Features)       │ ← CQRS Handlers
├─────────────────────────────────────────┤
│        Domain Layer (Entities)          │ ← Business Logic
├─────────────────────────────────────────┤
│    Infrastructure Layer (DbContext)     │ ← Data Access
├─────────────────────────────────────────┤
│     Building Blocks (Shared Services)   │ ← Cross-cutting
└─────────────────────────────────────────┘
```

---

## 🧪 Testing Quick Start

### Unit Test
```csharp
[Fact]
public async Task CreateYourEntity_ValidCommand_ReturnsId()
{
    // Arrange
    var handler = new CreateYourEntityCommandHandler(/* deps */);
    var command = new CreateYourEntityCommand("Test");
    
    // Act
    var result = await handler.Handle(command, CancellationToken.None);
    
    // Assert
    result.Should().NotBe(Guid.Empty);
}
```

### Integration Test
```csharp
[Fact]
public async Task CreateYourEntity_ReturnsCreated()
{
    // Arrange
    var client = _factory.CreateClient();
    
    // Act
    var response = await client.PostAsJsonAsync(
        "/api/v1/your-entities", 
        new CreateYourEntityCommand("Test"));
    
    // Assert
    response.StatusCode.Should().Be(HttpStatusCode.Created);
}
```

---

## 🚨 Common Issues & Solutions

| Issue | Solution |
|-------|----------|
| Redis timeout | Use `make api-http` (no Redis needed) or `make apphost` |
| Port in use | `make kill-ports` |
| HTTPS error | `make setup` |
| Blazor can't reach API | Ensure API is running first |
| Migration failed | Check connection string and context name |
| NSwag not found | `dotnet tool install --global NSwag.ConsoleCore` |

---

## 📚 Key Files

| File | Purpose |
|------|---------|
| `Program.cs` | Host configuration & module registration |
| `{Module}Module.cs` | Module services & endpoint registration |
| `appsettings.json` | Configuration (connection strings, etc.) |
| `Makefile` | Build & run commands |
| `Directory.Build.props` | Shared project settings |
| `Directory.Packages.props` | Centralized package versions |

---

## 🎓 Learning Path

1. ✅ Read `ARCHITECTURE_GUIDE.md`
2. ✅ Read this Quick Reference
3. ✅ Study existing modules (Identity, Multitenancy)
4. ✅ Create your first module
5. ✅ Add a Blazor page
6. ✅ Read `EXPANSION_GUIDE.md` for advanced patterns

---

**You're ready to build! Start with a simple CRUD module and expand from there.** 🚀

