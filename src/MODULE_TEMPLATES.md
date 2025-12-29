# FSH Framework - Decision Tree & Module Templates

## 📋 Module Type Decision Tree

```
START: What are you building?
│
├─ Simple data management? (CRUD)
│  └─ Use: BASIC CRUD MODULE
│     Examples: Categories, Tags, Settings
│     Template: Template A (below)
│
├─ Complex business workflows?
│  └─ Use: WORKFLOW MODULE
│     Examples: Orders, Approvals, Tickets
│     Template: Template B (below)
│
├─ File/media handling?
│  └─ Use: FILE MODULE
│     Examples: Documents, Images, Attachments
│     Template: Template C (below)
│
├─ External API integration?
│  └─ Use: INTEGRATION MODULE
│     Examples: Payment Gateway, Shipping API
│     Template: Template D (below)
│
├─ Heavy read operations?
│  └─ Use: CACHED MODULE
│     Examples: Product Catalog, Lookup Data
│     Template: Template E (below)
│
├─ Time-series or analytics?
│  └─ Use: REPORTING MODULE
│     Examples: Sales Reports, Metrics Dashboard
│     Template: Template F (below)
│
└─ Background processing?
   └─ Use: JOB MODULE
      Examples: Batch Processing, Scheduled Tasks
      Template: Template G (below)
```

---

## Template A: Basic CRUD Module

**Use When:** Simple entity with create, read, update, delete operations

**Example:** Categories Module

### File Structure
```
Modules/Categories/
├── Modules.Categories.Contracts/
│   ├── Modules.Categories.Contracts.csproj
│   ├── DTOs/
│   │   └── CategoryDto.cs
│   └── v1/
│       ├── CreateCategory/
│       │   └── CreateCategoryCommand.cs
│       ├── UpdateCategory/
│       │   └── UpdateCategoryCommand.cs
│       ├── DeleteCategory/
│       │   └── DeleteCategoryCommand.cs
│       └── GetCategories/
│           ├── GetCategoriesQuery.cs
│           └── GetCategoryByIdQuery.cs
│
└── Modules.Categories/
    ├── Modules.Categories.csproj
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
            │   ├── CreateCategoryCommandHandler.cs
            │   ├── CreateCategoryCommandValidator.cs
            │   └── CreateCategoryEndpoint.cs
            ├── UpdateCategory/
            │   ├── UpdateCategoryCommandHandler.cs
            │   ├── UpdateCategoryCommandValidator.cs
            │   └── UpdateCategoryEndpoint.cs
            ├── DeleteCategory/
            │   ├── DeleteCategoryCommandHandler.cs
            │   └── DeleteCategoryEndpoint.cs
            └── GetCategories/
                ├── GetCategoriesQueryHandler.cs
                ├── GetCategoryByIdQueryHandler.cs
                ├── GetCategoriesEndpoint.cs
                └── GetCategoryByIdEndpoint.cs
```

### Code Templates

**Domain Entity:**
```csharp
public class Category : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Multi-tenancy
    public string TenantId { get; set; } = default!;
    
    // Audit
    public DateTimeOffset CreatedOn { get; set; }
    public string? CreatedBy { get; set; }
    public DateTimeOffset? LastModifiedOn { get; set; }
    public string? LastModifiedBy { get; set; }
}
```

**Endpoints (all 5 operations):**
```csharp
public void MapEndpoints(IEndpointRouteBuilder endpoints)
{
    var group = endpoints.MapGroup("/api/v1/categories").WithTags("Categories");
    
    // Create
    group.MapPost("/", async (CreateCategoryCommand cmd, IMediator mediator) =>
        TypedResults.Created($"/api/v1/categories/{await mediator.Send(cmd)}", default))
        .RequireAuthorization("categories:create");
    
    // Get All
    group.MapGet("/", async (IMediator mediator) =>
        TypedResults.Ok(await mediator.Send(new GetCategoriesQuery())))
        .RequireAuthorization("categories:read");
    
    // Get By Id
    group.MapGet("/{id:guid}", async (Guid id, IMediator mediator) =>
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id));
        return result is not null ? TypedResults.Ok(result) : TypedResults.NotFound();
    }).RequireAuthorization("categories:read");
    
    // Update
    group.MapPut("/{id:guid}", async (Guid id, UpdateCategoryCommand cmd, IMediator mediator) =>
    {
        if (id != cmd.Id) return TypedResults.BadRequest();
        await mediator.Send(cmd);
        return TypedResults.NoContent();
    }).RequireAuthorization("categories:update");
    
    // Delete
    group.MapDelete("/{id:guid}", async (Guid id, IMediator mediator) =>
    {
        await mediator.Send(new DeleteCategoryCommand(id));
        return TypedResults.NoContent();
    }).RequireAuthorization("categories:delete");
}
```

**Estimated Time:** 2-3 hours

---

## Template B: Workflow Module

**Use When:** Complex state management, approvals, or business processes

**Example:** Orders Module

### Additional Files
```
Features/v1/Orders/
├── PlaceOrder/              # Initial state
├── ConfirmOrder/            # State transition
├── CancelOrder/             # State transition
├── ShipOrder/               # State transition
└── CompleteOrder/           # Final state
```

### State Machine Pattern
```csharp
public class Order : IAuditableEntity, IMustHaveTenant
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    
    // State transition rules
    public bool CanConfirm() => Status == OrderStatus.Pending;
    public bool CanCancel() => Status is OrderStatus.Pending or OrderStatus.Confirmed;
    public bool CanShip() => Status == OrderStatus.Confirmed;
    
    // State transition methods
    public void Confirm()
    {
        if (!CanConfirm())
            throw new InvalidOperationException("Cannot confirm order in current state");
        
        Status = OrderStatus.Confirmed;
        // Raise domain event
        RaiseDomainEvent(new OrderConfirmedEvent(Id));
    }
    
    private List<DomainEvent> _domainEvents = new();
    public IReadOnlyList<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    
    public void RaiseDomainEvent(DomainEvent @event)
    {
        _domainEvents.Add(@event);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}

public enum OrderStatus
{
    Draft,
    Pending,
    Confirmed,
    Shipped,
    Delivered,
    Cancelled
}
```

### Domain Event Handler
```csharp
public class OrderConfirmedEventHandler : INotificationHandler<OrderConfirmedEvent>
{
    private readonly IEventPublisher _eventPublisher;
    
    public async Task Handle(OrderConfirmedEvent notification, CancellationToken ct)
    {
        // Publish integration event for other modules
        await _eventPublisher.PublishAsync(
            new OrderConfirmedIntegrationEvent(
                notification.OrderId,
                notification.ConfirmedAt),
            ct);
    }
}
```

**Estimated Time:** 4-6 hours

---

## Template C: File Module

**Use When:** Handling file uploads, downloads, or document management

**Example:** Documents Module

### Additional Files
```
Features/v1/Documents/
├── UploadDocument/
├── DownloadDocument/
├── DeleteDocument/
└── GetDocuments/

Services/
└── DocumentStorageService.cs
```

### File Upload Pattern
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
    
    // Allowed file types
    private static readonly string[] AllowedTypes = 
    {
        "application/pdf",
        "image/jpeg",
        "image/png",
        "application/msword",
        "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
    };
    
    public async ValueTask<Guid> Handle(UploadDocumentCommand command, CancellationToken ct)
    {
        // 1. Validate file type
        if (!AllowedTypes.Contains(command.ContentType))
            throw new ValidationException("File type not allowed");
        
        // 2. Validate file size (10MB max)
        if (command.Content.Length > 10 * 1024 * 1024)
            throw new ValidationException("File too large (max 10MB)");
        
        // 3. Generate unique filename
        var extension = Path.GetExtension(command.FileName);
        var uniqueFileName = $"{Guid.NewGuid()}{extension}";
        
        // 4. Upload to storage
        var fileUrl = await _storage.UploadAsync(
            uniqueFileName,
            command.Content,
            command.ContentType,
            ct);
        
        // 5. Save metadata
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
}
```

### Blazor Upload Component
```razor
<InputFile OnChange="HandleFileUpload" accept=".pdf,.jpg,.png" />

@code {
    private async Task HandleFileUpload(InputFileChangeEventArgs e)
    {
        var file = e.File;
        
        using var stream = file.OpenReadStream(maxAllowedSize: 10 * 1024 * 1024);
        
        var command = new UploadDocumentCommand(
            file.Name,
            file.ContentType,
            stream,
            DocumentCategory.General
        );
        
        var id = await ApiClient.UploadDocumentAsync(command);
        
        Snackbar.Add("File uploaded successfully", Severity.Success);
    }
}
```

**Estimated Time:** 3-4 hours

---

## Template D: Integration Module

**Use When:** Integrating with external APIs or services

**Example:** Payment Gateway Module

### Additional Files
```
Services/
├── IPaymentProvider.cs
├── StripePaymentProvider.cs
└── PayPalPaymentProvider.cs

Options/
└── PaymentOptions.cs
```

### External API Pattern
```csharp
public interface IPaymentProvider
{
    Task<PaymentResult> ProcessPaymentAsync(PaymentRequest request, CancellationToken ct);
    Task<RefundResult> RefundPaymentAsync(string paymentId, decimal amount, CancellationToken ct);
}

public class StripePaymentProvider : IPaymentProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<StripePaymentProvider> _logger;
    private readonly PaymentOptions _options;
    
    public async Task<PaymentResult> ProcessPaymentAsync(
        PaymentRequest request, 
        CancellationToken ct)
    {
        try
        {
            // Use Polly for resilience
            var policy = Policy
                .Handle<HttpRequestException>()
                .WaitAndRetryAsync(3, retryAttempt => 
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            
            return await policy.ExecuteAsync(async () =>
            {
                var response = await _httpClient.PostAsJsonAsync(
                    "https://api.stripe.com/v1/charges",
                    new
                    {
                        amount = request.Amount * 100, // Convert to cents
                        currency = request.Currency,
                        source = request.Token
                    },
                    ct);
                
                response.EnsureSuccessStatusCode();
                
                var result = await response.Content.ReadFromJsonAsync<StripeChargeResponse>(ct);
                
                return new PaymentResult
                {
                    Success = true,
                    TransactionId = result.Id,
                    Amount = request.Amount
                };
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Payment processing failed");
            
            return new PaymentResult
            {
                Success = false,
                ErrorMessage = "Payment processing failed"
            };
        }
    }
}
```

### Circuit Breaker Pattern
```csharp
services.AddHttpClient<IPaymentProvider, StripePaymentProvider>()
    .AddTransientHttpErrorPolicy(policyBuilder =>
        policyBuilder.CircuitBreakerAsync(
            handledEventsAllowedBeforeBreaking: 5,
            durationOfBreak: TimeSpan.FromSeconds(30)
        ));
```

**Estimated Time:** 4-5 hours

---

## Template E: Cached Module

**Use When:** High read traffic, infrequent updates

**Example:** Product Catalog Module

### Caching Pattern
```csharp
public class GetProductsQueryHandler : IQueryHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly ICacheService _cache;
    private readonly ProductsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<List<ProductDto>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var cacheKey = $"products:tenant:{_currentUser.TenantId}:category:{query.Category}";
        
        // Try cache first (cache-aside pattern)
        return await _cache.GetOrSetAsync(
            cacheKey,
            async () =>
            {
                // Load from database if not in cache
                return await _db.Products
                    .Where(p => p.Category == query.Category)
                    .OrderBy(p => p.Name)
                    .Select(p => new ProductDto(p.Id, p.Name, p.Price))
                    .ToListAsync(ct);
            },
            TimeSpan.FromMinutes(10), // Cache for 10 minutes
            ct);
    }
}

// Invalidate cache on update
public class UpdateProductCommandHandler : ICommandHandler<UpdateProductCommand>
{
    private readonly ICacheService _cache;
    private readonly ProductsDbContext _db;
    private readonly ICurrentUser _currentUser;
    
    public async ValueTask<Unit> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await _db.Products.FindAsync(command.Id, ct);
        product.Update(command);
        
        await _db.SaveChangesAsync(ct);
        
        // Invalidate all related cache entries
        var pattern = $"products:tenant:{_currentUser.TenantId}:*";
        await _cache.RemoveByPatternAsync(pattern, ct);
        
        return Unit.Value;
    }
}
```

**Estimated Time:** 3-4 hours

---

## Template F: Reporting Module

**Use When:** Analytics, dashboards, or complex queries

**Example:** Sales Reports Module

### Reporting Pattern
```csharp
public class GetSalesReportQueryHandler : IQueryHandler<GetSalesReportQuery, SalesReportDto>
{
    private readonly ReportsDbContext _db;
    
    public async ValueTask<SalesReportDto> Handle(GetSalesReportQuery query, CancellationToken ct)
    {
        var startDate = query.StartDate;
        var endDate = query.EndDate;
        
        // Use raw SQL for complex aggregations
        var dailySales = await _db.Database
            .SqlQueryRaw<DailySalesDto>(@"
                SELECT 
                    DATE(created_on) as date,
                    COUNT(*) as order_count,
                    SUM(total_amount) as total_amount
                FROM orders
                WHERE tenant_id = {0}
                  AND created_on >= {1}
                  AND created_on <= {2}
                  AND status != 'Cancelled'
                GROUP BY DATE(created_on)
                ORDER BY DATE(created_on)",
                _currentUser.TenantId,
                startDate,
                endDate)
            .ToListAsync(ct);
        
        // Calculate summary
        return new SalesReportDto
        {
            Period = $"{startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}",
            TotalOrders = dailySales.Sum(d => d.OrderCount),
            TotalRevenue = dailySales.Sum(d => d.TotalAmount),
            AverageOrderValue = dailySales.Average(d => d.TotalAmount),
            DailySales = dailySales
        };
    }
}
```

### Background Job for Pre-computation
```csharp
public class GenerateDailySalesReportJob
{
    private readonly ReportsDbContext _db;
    
    [DisableConcurrentExecution(timeoutInSeconds: 300)]
    public async Task ExecuteAsync(CancellationToken ct)
    {
        // Pre-compute yesterday's report
        var yesterday = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1));
        
        // Generate and cache report
        // ...
    }
}
```

**Estimated Time:** 4-6 hours

---

## Template G: Job Module

**Use When:** Background processing, scheduled tasks

**Example:** Email Notification Module

### Job Pattern
```csharp
public class SendDailyDigestJob
{
    private readonly IEmailService _emailService;
    private readonly IUserService _userService;
    
    [AutomaticRetry(Attempts = 3)]
    [DisableConcurrentExecution(timeoutInSeconds: 600)]
    public async Task ExecuteAsync(CancellationToken ct)
    {
        var users = await _userService.GetUsersWithDigestEnabledAsync(ct);
        
        foreach (var user in users)
        {
            try
            {
                var digest = await BuildDigestAsync(user.Id, ct);
                await _emailService.SendAsync(user.Email, "Daily Digest", digest, ct);
            }
            catch (Exception ex)
            {
                // Log and continue with next user
                _logger.LogError(ex, "Failed to send digest to {Email}", user.Email);
            }
        }
    }
}

// Register in Module
public void ConfigureServices(IHostApplicationBuilder builder)
{
    // ... other services ...
    
    // Schedule recurring job (runs daily at 9 AM)
    RecurringJob.AddOrUpdate<SendDailyDigestJob>(
        "send-daily-digest",
        job => job.ExecuteAsync(CancellationToken.None),
        "0 9 * * *"); // Cron expression
}
```

**Estimated Time:** 2-3 hours

---

## Quick Decision Table

| Need | Template | Time | Complexity |
|------|----------|------|------------|
| Simple CRUD | A | 2-3h | ⭐ |
| State Machine | B | 4-6h | ⭐⭐⭐ |
| File Upload | C | 3-4h | ⭐⭐ |
| External API | D | 4-5h | ⭐⭐⭐ |
| High-Read Cache | E | 3-4h | ⭐⭐ |
| Analytics/Reports | F | 4-6h | ⭐⭐⭐ |
| Background Jobs | G | 2-3h | ⭐⭐ |

---

## Module Combination Examples

### E-Commerce Order System
```
Orders Module (Template B: Workflow)
  + Payments Module (Template D: Integration)
  + Inventory Module (Template A: CRUD)
  + Shipping Module (Template D: Integration)
```

### Document Management System
```
Documents Module (Template C: File)
  + Folders Module (Template A: CRUD)
  + Versions Module (Template A: CRUD)
  + Search Module (Template E: Cached)
```

### CRM System
```
Contacts Module (Template A: CRUD)
  + Opportunities Module (Template B: Workflow)
  + Activities Module (Template A: CRUD)
  + Reports Module (Template F: Reporting)
  + Email Sync Module (Template G: Job)
```

---

## Next Steps

1. ✅ **Choose your template** based on requirements
2. ✅ **Follow the structure** provided
3. ✅ **Start simple** (Template A) if unsure
4. ✅ **Test thoroughly** as you build
5. ✅ **Iterate** and add complexity as needed

**Pro Tip:** Start with Template A (CRUD), get it working, then add features from other templates as needed!

