# FSH Framework - Expansion Roadmap & Patterns

## Table of Contents
1. [Common Patterns](#common-patterns)
2. [Real-World Examples](#real-world-examples)
3. [Expansion Roadmap](#expansion-roadmap)
4. [Performance Optimization](#performance-optimization)
5. [Testing Strategy](#testing-strategy)

---

## Common Patterns

### **Pattern 1: Simple CRUD Module**

Use this for basic entities with standard create/read/update/delete operations.

**Example: Categories Module**

```
Modules/Categories/
├── Modules.Categories.Contracts/
│   └── v1/
│       ├── CreateCategoryCommand.cs
│       ├── UpdateCategoryCommand.cs
│       ├── DeleteCategoryCommand.cs
│       ├── GetCategoriesQuery.cs
│       └── CategoryResponse.cs
└── Modules.Categories/
    ├── CategoriesModule.cs
    ├── Domain/
    │   └── Category.cs
    ├── Data/
    │   ├── CategoriesDbContext.cs
    │   └── Configurations/
    │       └── CategoryConfiguration.cs
    └── Features/
        └── v1/
            ├── CreateCategory/
            ├── UpdateCategory/
            ├── DeleteCategory/
            └── GetCategories/
```

### **Pattern 2: Module with Business Rules**

Use this when you have complex business logic, workflows, or state machines.

**Example: Orders Module**

```csharp
// Domain Entity with State
public class Order : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public OrderStatus Status { get; set; } // Enum: Draft, Pending, Confirmed, Shipped, Delivered, Cancelled
    public decimal TotalAmount { get; set; }
    public List<OrderItem> Items { get; set; } = new();
    
    // Business Rules
    public bool CanBeCancelled() => Status is OrderStatus.Draft or OrderStatus.Pending;
    public bool CanBeShipped() => Status == OrderStatus.Confirmed;
    
    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
            throw new InvalidOperationException("Only pending orders can be confirmed");
        
        Status = OrderStatus.Confirmed;
        // Raise domain event
    }
    
    public void Cancel(string reason)
    {
        if (!CanBeCancelled())
            throw new InvalidOperationException("Order cannot be cancelled");
        
        Status = OrderStatus.Cancelled;
        // Publish integration event for inventory restoration
    }
}

// Domain Event
public record OrderConfirmedEvent(Guid OrderId, DateTimeOffset ConfirmedAt) : DomainEvent;

// Integration Event (for other modules)
public record OrderConfirmedIntegrationEvent(
    Guid OrderId, 
    string TenantId, 
    List<OrderItemDto> Items) : IntegrationEvent;
```

### **Pattern 3: Module with External Integration**

Use this when integrating with third-party services.

**Example: Notifications Module (Email/SMS)**

```csharp
// Service Interface
public interface INotificationService
{
    Task SendEmailAsync(EmailMessage message, CancellationToken ct);
    Task SendSmsAsync(SmsMessage message, CancellationToken ct);
    Task SendPushNotificationAsync(PushMessage message, CancellationToken ct);
}

// Implementation with Circuit Breaker
public class NotificationService : INotificationService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<NotificationService> _logger;
    
    public async Task SendEmailAsync(EmailMessage message, CancellationToken ct)
    {
        var client = _httpClientFactory.CreateClient("EmailProvider");
        
        // Use Polly for resilience
        var response = await Policy
            .Handle<HttpRequestException>()
            .Or<TaskCanceledException>()
            .WaitAndRetryAsync(3, retryAttempt => 
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)))
            .ExecuteAsync(() => client.PostAsJsonAsync("/send", message, ct));
        
        response.EnsureSuccessStatusCode();
    }
}

// Background Job for Retry
public class FailedNotificationRetryJob
{
    [AutomaticRetry(Attempts = 5)]
    public async Task RetryFailedNotifications(CancellationToken ct)
    {
        // Retry logic
    }
}
```

### **Pattern 4: Read-Heavy Module with Caching**

Use this for modules with high read traffic and infrequent updates.

**Example: Settings Module**

```csharp
public class GetTenantSettingsQueryHandler : IQueryHandler<GetTenantSettingsQuery, SettingsResponse>
{
    private readonly ICacheService _cache;
    private readonly SettingsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<SettingsResponse> Handle(GetTenantSettingsQuery query, CancellationToken ct)
    {
        var cacheKey = $"settings:tenant:{_currentUser.TenantId}";
        
        // Try cache first
        var cached = await _cache.GetAsync<SettingsResponse>(cacheKey, ct);
        if (cached is not null)
            return cached;
        
        // Load from database
        var settings = await _db.Settings
            .Where(s => s.TenantId == _currentUser.TenantId)
            .Select(s => new SettingsResponse(s.Theme, s.TimeZone, s.Currency))
            .FirstOrDefaultAsync(ct);
        
        // Cache for 1 hour
        await _cache.SetAsync(cacheKey, settings, TimeSpan.FromHours(1), ct);
        
        return settings;
    }
}

// Invalidate cache on update
public class UpdateSettingsCommandHandler : ICommandHandler<UpdateSettingsCommand>
{
    private readonly ICacheService _cache;
    private readonly SettingsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<Unit> Handle(UpdateSettingsCommand command, CancellationToken ct)
    {
        var settings = await _db.Settings.FindAsync(command.Id, ct);
        settings.Update(command);
        
        await _db.SaveChangesAsync(ct);
        
        // Invalidate cache
        var cacheKey = $"settings:tenant:{_currentUser.TenantId}";
        await _cache.RemoveAsync(cacheKey, ct);
        
        return Unit.Value;
    }
}
```

### **Pattern 5: Module with File Upload**

Use this for handling file uploads (images, documents, etc.).

**Example: Documents Module**

```csharp
public record UploadDocumentCommand(
    string FileName,
    string ContentType,
    Stream Content,
    DocumentCategory Category
) : ICommand<Guid>;

public class UploadDocumentCommandHandler : ICommandHandler<UploadDocumentCommand, Guid>
{
    private readonly IFileStorageService _storage;
    private readonly DocumentsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<Guid> Handle(UploadDocumentCommand command, CancellationToken ct)
    {
        // Validate file type
        if (!IsAllowedFileType(command.ContentType))
            throw new ValidationException("File type not allowed");
        
        // Validate file size (10MB max)
        if (command.Content.Length > 10 * 1024 * 1024)
            throw new ValidationException("File too large");
        
        // Generate unique file name
        var extension = Path.GetExtension(command.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        
        // Upload to storage (S3/Azure Blob/Local)
        var fileUrl = await _storage.UploadAsync(
            uniqueFileName,
            command.Content,
            command.ContentType,
            ct);
        
        // Save metadata to database
        var document = new Document
        {
            Id = Guid.NewGuid(),
            FileName = command.FileName,
            StorageFileName = uniqueFileName,
            ContentType = command.ContentType,
            FileSize = command.Content.Length,
            FileUrl = fileUrl,
            Category = command.Category,
            TenantId = _currentUser.TenantId!
        };
        
        _db.Documents.Add(document);
        await _db.SaveChangesAsync(ct);
        
        return document.Id;
    }
    
    private bool IsAllowedFileType(string contentType)
    {
        var allowed = new[] { "application/pdf", "image/jpeg", "image/png", "application/msword" };
        return allowed.Contains(contentType);
    }
}
```

### **Pattern 6: Module with Bulk Operations**

Use this for handling large datasets efficiently.

**Example: Bulk Import**

```csharp
public record ImportProductsCommand(Stream CsvFile) : ICommand<ImportResult>;

public class ImportProductsCommandHandler : ICommandHandler<ImportProductsCommand, ImportResult>
{
    private readonly ProductsDbContext _db;
    private readonly ILogger<ImportProductsCommandHandler> _logger;
    
    public async ValueTask<ImportResult> Handle(ImportProductsCommand command, CancellationToken ct)
    {
        var result = new ImportResult();
        var batch = new List<Product>();
        const int batchSize = 1000;
        
        using var reader = new StreamReader(command.CsvFile);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        
        await foreach (var record in csv.GetRecordsAsync<ProductCsvRow>(ct))
        {
            try
            {
                var product = MapToProduct(record);
                batch.Add(product);
                
                // Process in batches for performance
                if (batch.Count >= batchSize)
                {
                    await ProcessBatchAsync(batch, ct);
                    result.SuccessCount += batch.Count;
                    batch.Clear();
                }
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Row {csv.Parser.Row}: {ex.Message}");
                _logger.LogWarning(ex, "Failed to import product at row {Row}", csv.Parser.Row);
            }
        }
        
        // Process remaining items
        if (batch.Any())
        {
            await ProcessBatchAsync(batch, ct);
            result.SuccessCount += batch.Count;
        }
        
        return result;
    }
    
    private async Task ProcessBatchAsync(List<Product> batch, CancellationToken ct)
    {
        _db.Products.AddRange(batch);
        await _db.SaveChangesAsync(ct);
    }
}

// Queue as background job for large files
public class BulkImportJob
{
    private readonly IMediator _mediator;
    
    [Queue("bulk-operations")]
    [AutomaticRetry(Attempts = 3)]
    public async Task ImportProductsAsync(string filePath, string tenantId, CancellationToken ct)
    {
        // Set tenant context
        using var fileStream = File.OpenRead(filePath);
        await _mediator.Send(new ImportProductsCommand(fileStream), ct);
        
        // Clean up temp file
        File.Delete(filePath);
    }
}
```

---

## Real-World Examples

### **Example 1: E-Commerce Platform**

```
Modules/
├── Catalog/              # Product catalog
│   ├── Products/
│   ├── Categories/
│   └── Brands/
├── Inventory/            # Stock management
│   ├── StockItems/
│   ├── Warehouses/
│   └── StockMovements/
├── Orders/               # Order processing
│   ├── Orders/
│   ├── OrderItems/
│   └── OrderTracking/
├── Payments/             # Payment processing
│   ├── PaymentMethods/
│   ├── Transactions/
│   └── Refunds/
├── Shipping/             # Logistics
│   ├── ShippingMethods/
│   ├── Shipments/
│   └── TrackingEvents/
├── Customers/            # Customer management
│   ├── Customers/
│   ├── Addresses/
│   └── Wishlists/
├── Reviews/              # Product reviews
│   ├── Reviews/
│   └── Ratings/
└── Analytics/            # Business intelligence
    ├── SalesReports/
    └── CustomerInsights/
```

**Integration Flow:**
```
Order Created → Check Inventory → Reserve Stock → Process Payment → Create Shipment → Send Notifications
```

### **Example 2: CRM System**

```
Modules/
├── Contacts/             # Contact management
│   ├── Contacts/
│   ├── Companies/
│   └── ContactNotes/
├── Leads/                # Lead tracking
│   ├── Leads/
│   ├── LeadSources/
│   └── LeadScoring/
├── Opportunities/        # Sales pipeline
│   ├── Opportunities/
│   ├── OpportunityStages/
│   └── Forecasts/
├── Activities/           # Task & calendar
│   ├── Tasks/
│   ├── Appointments/
│   └── Calls/
├── Campaigns/            # Marketing campaigns
│   ├── Campaigns/
│   ├── EmailTemplates/
│   └── CampaignMembers/
└── Reports/              # Analytics
    ├── SalesReports/
    └── ActivityReports/
```

### **Example 3: Project Management Tool**

```
Modules/
├── Projects/             # Project management
│   ├── Projects/
│   ├── Milestones/
│   └── ProjectMembers/
├── Tasks/                # Task management
│   ├── Tasks/
│   ├── TaskAssignments/
│   └── TaskComments/
├── TimeTracking/         # Time logging
│   ├── TimeEntries/
│   └── Timesheets/
├── Documents/            # File management
│   ├── Documents/
│   ├── Folders/
│   └── DocumentVersions/
├── Workspaces/           # Team workspaces
│   └── Workspaces/
└── Reporting/            # Project analytics
    ├── ProgressReports/
    └── ResourceUtilization/
```

---

## Expansion Roadmap

### **Phase 1: Foundation (Months 1-2)**
✅ Core modules operational (Identity, Multitenancy, Auditing)
✅ Basic CRUD operations
✅ Authentication & Authorization
✅ API documentation

**Next Steps:**
- [ ] Add your first custom module
- [ ] Implement basic search functionality
- [ ] Add file upload capability
- [ ] Set up automated testing

### **Phase 2: Feature Expansion (Months 3-4)**

**Add Advanced Features:**
```
1. Full-Text Search
   - Integrate Elasticsearch or PostgreSQL FTS
   - Create search index
   - Implement search API

2. Notifications System
   - Email notifications
   - In-app notifications
   - Push notifications (Firebase, SignalR)

3. Reporting & Analytics
   - Custom report builder
   - Dashboard widgets
   - Export to Excel/PDF

4. Advanced Workflows
   - Approval workflows
   - State machines
   - Business rule engine
```

**Code Example: Search Module**

```csharp
// Search Service
public interface ISearchService
{
    Task IndexAsync<T>(T entity) where T : class;
    Task<SearchResult<T>> SearchAsync<T>(SearchQuery query) where T : class;
}

// Elasticsearch Implementation
public class ElasticsearchSearchService : ISearchService
{
    private readonly IElasticClient _client;
    
    public async Task IndexAsync<T>(T entity) where T : class
    {
        await _client.IndexDocumentAsync(entity);
    }
    
    public async Task<SearchResult<T>> SearchAsync<T>(SearchQuery query) where T : class
    {
        var response = await _client.SearchAsync<T>(s => s
            .Query(q => q
                .MultiMatch(m => m
                    .Query(query.Term)
                    .Fields(f => f.Field("*"))))
            .From(query.Skip)
            .Size(query.Take));
        
        return new SearchResult<T>
        {
            Items = response.Documents.ToList(),
            Total = response.Total
        };
    }
}
```

### **Phase 3: Optimization (Months 5-6)**

**Performance Improvements:**
```
1. Caching Strategy
   - Implement distributed caching
   - Add cache warming
   - Use cache-aside pattern

2. Database Optimization
   - Add missing indexes
   - Implement read replicas
   - Query optimization
   - Connection pooling tuning

3. API Performance
   - Response compression
   - Output caching
   - GraphQL for complex queries
   - Batch endpoints

4. Frontend Optimization
   - Lazy loading
   - Virtual scrolling
   - Image optimization
   - Bundle optimization
```

### **Phase 4: Scalability (Months 7-9)**

**Prepare for Scale:**
```
1. Move to Distributed Architecture
   - Split modules into services
   - Add API Gateway
   - Service discovery

2. Implement Message Queue
   - RabbitMQ or Azure Service Bus
   - Event-driven architecture
   - Async processing

3. Add Monitoring & Observability
   - Application Performance Monitoring (APM)
   - Distributed tracing
   - Log aggregation
   - Alerts & notifications

4. High Availability
   - Load balancing
   - Auto-scaling
   - Disaster recovery
   - Backup automation
```

### **Phase 5: Enterprise Features (Months 10-12)**

**Enterprise Readiness:**
```
1. Advanced Security
   - Two-factor authentication
   - Single Sign-On (SSO)
   - OAuth2/OIDC providers
   - Security audit logs

2. Compliance
   - GDPR compliance
   - Data export/import
   - Right to be forgotten
   - Audit trails

3. Multi-Language Support
   - Localization (i18n)
   - RTL support
   - Date/time formatting
   - Currency conversion

4. Advanced Integrations
   - Webhooks
   - API marketplace
   - Third-party connectors
   - iPaaS integration
```

---

## Performance Optimization

### **Database Optimization**

**1. Indexing Strategy**

```csharp
// Add indexes in entity configuration
public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products", "catalog");
        
        // Primary key
        builder.HasKey(x => x.Id);
        
        // Indexes for common queries
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => x.Category);
        builder.HasIndex(x => new { x.TenantId, x.Category }); // Composite index
        builder.HasIndex(x => x.CreatedOn);
        
        // Full-text search (PostgreSQL)
        builder.HasIndex(x => new { x.Name, x.Description })
            .HasMethod("GIN")
            .IsTsVectorExpressionIndex("english");
    }
}
```

**2. Query Optimization**

```csharp
// ❌ BAD: N+1 query problem
public async Task<List<OrderDto>> GetOrdersAsync()
{
    var orders = await _db.Orders.ToListAsync();
    return orders.Select(o => new OrderDto
    {
        Id = o.Id,
        CustomerName = o.Customer.Name, // Extra query!
        Items = o.Items.Select(i => ...).ToList() // Extra query per order!
    }).ToList();
}

// ✅ GOOD: Use eager loading
public async Task<List<OrderDto>> GetOrdersAsync()
{
    return await _db.Orders
        .Include(o => o.Customer)
        .Include(o => o.Items)
        .Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.Customer.Name,
            Items = o.Items.Select(i => new OrderItemDto
            {
                ProductName = i.Product.Name,
                Quantity = i.Quantity
            }).ToList()
        })
        .ToListAsync();
}

// ✅ BETTER: Use projection (no tracking)
public async Task<List<OrderDto>> GetOrdersAsync()
{
    return await _db.Orders
        .AsNoTracking()
        .Select(o => new OrderDto
        {
            Id = o.Id,
            CustomerName = o.Customer.Name,
            ItemCount = o.Items.Count
        })
        .ToListAsync();
}
```

**3. Pagination**

```csharp
public record PaginatedResult<T>(List<T> Items, int Total, int Page, int PageSize);

public async Task<PaginatedResult<ProductDto>> GetProductsAsync(int page, int pageSize)
{
    var query = _db.Products.AsQueryable();
    
    var total = await query.CountAsync();
    
    var items = await query
        .OrderBy(p => p.Name)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new ProductDto(p.Id, p.Name, p.Price))
        .ToListAsync();
    
    return new PaginatedResult<ProductDto>(items, total, page, pageSize);
}
```

### **Caching Strategies**

**1. Response Caching**

```csharp
// Endpoint with output caching
public static RouteHandlerBuilder MapGetProductsEndpoint(this IEndpointRouteBuilder endpoint)
{
    return endpoint.MapGet("/products",
        async (ProductsDbContext db) =>
        {
            var products = await db.Products
                .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                .ToListAsync();
            return TypedResults.Ok(products);
        })
        .CacheOutput(policy => policy.Expire(TimeSpan.FromMinutes(5)));
}
```

**2. Distributed Cache**

```csharp
public class GetProductQueryHandler : IQueryHandler<GetProductQuery, ProductResponse>
{
    private readonly ICacheService _cache;
    private readonly ProductsDbContext _db;
    
    public async ValueTask<ProductResponse> Handle(GetProductQuery query, CancellationToken ct)
    {
        var cacheKey = $"product:{query.ProductId}";
        
        // Try cache first
        return await _cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                var product = await _db.Products.FindAsync(query.ProductId);
                return product.ToResponse();
            },
            TimeSpan.FromMinutes(10),
            ct);
    }
}
```

### **API Performance**

**1. Response Compression**

Already configured in `Playground.Blazor/Program.cs`:
```csharp
builder.Services.AddResponseCompression(options =>
{
    options.EnableForHttps = true;
    options.Providers.Add<BrotliCompressionProvider>();
    options.Providers.Add<GzipCompressionProvider>();
});
```

**2. Async All The Way**

```csharp
// ✅ GOOD: Fully async
public async Task<List<Product>> GetProductsAsync()
{
    return await _db.Products.ToListAsync();
}

// ❌ BAD: Blocking async
public List<Product> GetProducts()
{
    return _db.Products.ToListAsync().Result; // Deadlock risk!
}
```

---

## Testing Strategy

### **Unit Tests**

```csharp
public class CreateProductCommandHandlerTests
{
    [Fact]
    public async Task Handle_ValidCommand_CreatesProduct()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<ProductsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        using var context = new ProductsDbContext(options, ...);
        var handler = new CreateProductCommandHandler(context, Mock.Of<ICurrentUser>());
        var command = new CreateProductCommand("Test Product", "Description", 10.99m, "Category");
        
        // Act
        var result = await handler.Handle(command, CancellationToken.None);
        
        // Assert
        result.Should().NotBe(Guid.Empty);
        context.Products.Should().HaveCount(1);
    }
}
```

### **Integration Tests**

```csharp
public class ProductsApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public ProductsApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    
    [Fact]
    public async Task CreateProduct_ReturnsCreated()
    {
        // Arrange
        var client = _factory.CreateClient();
        var command = new CreateProductCommand("Test", "Desc", 10m, "Cat");
        
        // Act
        var response = await client.PostAsJsonAsync("/api/v1/products", command);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }
}
```

### **Architecture Tests**

Already included in `Tests/Architecture.Tests/`:
```csharp
[Fact]
public void Modules_Should_Not_Reference_Other_Modules_Directly()
{
    var modules = Types.InAssembly(typeof(IdentityModule).Assembly);
    
    var result = modules
        .That()
        .ResideInNamespace("FSH.Modules.Identity")
        .Should()
        .NotHaveDependencyOn("FSH.Modules.Products")
        .GetResult();
    
    result.IsSuccessful.Should().BeTrue();
}
```

---

## Summary

This expansion guide provides:

✅ **7 Common Patterns** for different scenarios
✅ **3 Real-World Examples** (E-Commerce, CRM, Project Management)
✅ **5-Phase Roadmap** from MVP to Enterprise
✅ **Performance Optimization** strategies
✅ **Testing Strategy** with examples

### **Quick Decision Matrix**

| Scenario | Pattern | Key Components |
|----------|---------|----------------|
| Simple data management | CRUD Module | Entity, DbContext, CQRS |
| Complex workflows | Business Rules | State machine, Domain events |
| External APIs | Integration Module | HttpClient, Circuit breaker, Retry |
| High-traffic reads | Caching Module | Redis, Cache-aside |
| File handling | Upload Module | Storage service, Validation |
| Large datasets | Bulk Operations | Batching, Background jobs |

**You now have everything needed to expand this system!** 🚀

