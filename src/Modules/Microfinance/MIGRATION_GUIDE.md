# MicroFinance Module Migration Guide

## Overview
This guide documents the migration of the MicroFinance module from Clean Architecture (Domain/Application/Infrastructure) to Vertical Slice Architecture following the Todo module pattern.

## Migration Strategy

### Phase 1: Foundation (Core Entities - Priority 1)
**Start with these 5-10 core entities:**
1. **Member** - Core customer entity
2. **Branch** - Organization structure
3. **Staff** - Employee management
4. **MemberGroup** - Group lending
5. **Loan** - Loan accounts
6. **LoanProduct** - Loan products
7. **SavingsAccount** - Savings accounts
8. **SavingsProduct** - Savings products

### Phase 2: Loan Management (Priority 2)
9. LoanApplication
10. LoanSchedule
11. LoanRepayment
12. LoanCollateral
13. LoanGuarantor
14. LoanDisbursementTranche

### Phase 3: Collections & Risk (Priority 3)
15. CollectionCase
16. CollectionAction
17. CreditScore
18. KycDocument
19. AmlAlert

### Phase 4: Mobile & Digital (Priority 4)
20. MobileWallet
21. MobileTransaction
22. UssdSession
23. AgentBanking

### Phase 5: Extended Features (Priority 5)
24. InsurancePolicy
25. ShareAccount
26. FixedDeposit
27. InvestmentAccount
28. (... remaining 50+ entities)

## Target Module Structure

```
Modules.Microfinance/
├── MicrofinanceModule.cs              # Module registration
├── MicrofinancePermissionConstants.cs # Permissions
├── Data/
│   ├── MicrofinanceDbContext.cs
│   └── MicrofinanceDbInitializer.cs
├── Domain/                            # Domain entities (DDD style)
│   ├── Member.cs
│   ├── Branch.cs
│   ├── Loan.cs
│   ├── SavingsAccount.cs
│   └── ... (all domain entities)
└── Features/v1/                       # Vertical slices
    ├── Members/
    │   ├── CreateMember/
    │   │   ├── CreateMemberCommand.cs
    │   │   ├── CreateMemberHandler.cs
    │   │   ├── CreateMemberValidator.cs
    │   │   └── CreateMemberEndpoint.cs
    │   ├── GetMember/
    │   ├── GetMembers/
    │   ├── UpdateMember/
    │   ├── DeleteMember/
    │   └── SearchMembers/
    ├── Branches/
    │   └── (similar structure)
    ├── Loans/
    │   ├── CreateLoan/
    │   ├── ApproveLoan/
    │   ├── DisburseLoan/
    │   ├── RepayLoan/
    │   └── ... (loan operations)
    └── SavingsAccounts/
        └── (similar structure)

Modules.Microfinance.Contracts/
└── v1/
    ├── Members/
    │   ├── MemberDto.cs
    │   ├── CreateMemberCommand.cs
    │   ├── UpdateMemberCommand.cs
    │   └── GetMembersQuery.cs
    ├── Branches/
    ├── Loans/
    └── SavingsAccounts/
```

## Migration Pattern

### 1. Domain Entity Pattern
Domain entities remain largely unchanged but should:
- Inherit from `AuditableEntity` or `BaseEntity` from BuildingBlocks
- Use private setters and factory methods
- Contain domain logic and validation
- Emit domain events where applicable

**Example:**
```csharp
// Before (Clean Architecture)
public class Member
{
    public Guid Id { get; set; }
    public string MemberNumber { get; set; }
    public string FirstName { get; set; }
    // ... properties
}

// After (Vertical Slice)
public class Member : AuditableEntity
{
    public string MemberNumber { get; private set; } = default!;
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string PhoneNumber { get; private set; } = default!;
    // ... more properties
    
    private Member() { } // For EF Core
    
    public static Member Create(
        string memberNumber,
        string firstName,
        string lastName,
        string phoneNumber,
        string tenantId,
        Guid createdBy,
        string createdByUserName)
    {
        var member = new Member
        {
            Id = Guid.NewGuid(),
            MemberNumber = memberNumber,
            FirstName = firstName,
            LastName = lastName,
            PhoneNumber = phoneNumber,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName
        };
        
        // Add domain event if needed
        member.QueueDomainEvent(new MemberCreatedEvent(member.Id));
        
        return member;
    }
    
    public void UpdateDetails(string firstName, string lastName, string phoneNumber)
    {
        FirstName = firstName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
    }
}
```

### 2. Feature Slice Pattern (CQRS)

Each feature follows this structure:

**Command/Query:**
```csharp
public record CreateMemberCommand(
    string MemberNumber,
    string FirstName,
    string LastName,
    string PhoneNumber,
    string Email
) : ICommand<Guid>;
```

**Handler:**
```csharp
public class CreateMemberHandler(
    MicrofinanceDbContext context) : ICommandHandler<CreateMemberCommand, Guid>
{
    public async ValueTask<Guid> Handle(CreateMemberCommand command, CancellationToken ct)
    {
        var member = Member.Create(
            command.MemberNumber,
            command.FirstName,
            command.LastName,
            command.PhoneNumber,
            context.TenantInfo!.Identifier!,
            context.UserId,
            context.UserName);
        
        context.Members.Add(member);
        await context.SaveChangesAsync(ct);
        
        return member.Id;
    }
}
```

**Validator:**
```csharp
public class CreateMemberValidator : AbstractValidator<CreateMemberCommand>
{
    public CreateMemberValidator()
    {
        RuleFor(x => x.MemberNumber).NotEmpty().MaximumLength(50);
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PhoneNumber).NotEmpty().Matches(@"^\+?[1-9]\d{1,14}$");
    }
}
```

**Endpoint:**
```csharp
public static class CreateMemberEndpoint
{
    public static RouteHandlerBuilder MapCreateMemberEndpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            CreateMemberCommand command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created($"/api/v1/microfinance/members/{id}", id);
        })
        .WithName(nameof(CreateMemberEndpoint))
        .WithSummary("Create a new member")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.Members.Create);
    }
}
```

### 3. DbContext Pattern
```csharp
public class MicrofinanceDbContext(
    DbContextOptions<MicrofinanceDbContext> options,
    IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
    ICurrentUserService currentUserService,
    TimeProvider timeProvider)
    : HeroDbContext<MicrofinanceDbContext>(options, multiTenantContextAccessor, currentUserService, timeProvider)
{
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Branch> Branches => Set<Branch>();
    public DbSet<Loan> Loans => Set<Loan>();
    public DbSet<SavingsAccount> SavingsAccounts => Set<SavingsAccount>();
    // ... other DbSets

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.HasDefaultSchema("microfinance");
    }
}
```

### 4. Module Registration Pattern
```csharp
public class MicrofinanceModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(MicrofinancePermissionConstants.GetPermissions());

        // Register DbContext
        builder.Services.AddHeroDbContext<MicrofinanceDbContext>();

        // Register Db Initializer
        builder.Services.AddScoped<IDbInitializer, MicrofinanceDbInitializer>();

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<MicrofinanceDbContext>(
                name: "db:microfinance",
                failureStatus: HealthStatus.Unhealthy);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder memberGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/members")
            .WithTags("Members")
            .WithApiVersionSet(apiVersionSet);

        memberGroup.MapCreateMemberEndpoint();
        memberGroup.MapGetMembersEndpoint();
        memberGroup.MapGetMemberEndpoint();
        memberGroup.MapUpdateMemberEndpoint();
        memberGroup.MapDeleteMemberEndpoint();

        // Branch endpoints
        RouteGroupBuilder branchGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/branches")
            .WithTags("Branches")
            .WithApiVersionSet(apiVersionSet);
        
        branchGroup.MapCreateBranchEndpoint();
        // ... other branch endpoints

        // Loan endpoints
        RouteGroupBuilder loanGroup = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/loans")
            .WithTags("Loans")
            .WithApiVersionSet(apiVersionSet);
        
        loanGroup.MapCreateLoanEndpoint();
        // ... other loan endpoints
    }
}
```

## Migration Checklist

### For Each Entity:

#### 1. Domain Layer
- [ ] Copy entity from `MicroFinance.Domain/Entities/`
- [ ] Convert to `AuditableEntity` or `BaseEntity`
- [ ] Add private setters
- [ ] Create static factory method
- [ ] Add domain methods (Update, Delete, etc.)
- [ ] Add domain events if needed
- [ ] Create EF Core configuration (if complex mappings needed)

#### 2. Contracts Layer
- [ ] Create DTOs in `Modules.Microfinance.Contracts/v1/{Entity}/`
- [ ] Create commands (Create, Update, Delete)
- [ ] Create queries (Get, GetById, Search)
- [ ] Create response models

#### 3. Features Layer
- [ ] Create feature folder in `Features/v1/{Entity}/`
- [ ] For each operation (Create, Get, Update, Delete, Search):
  - [ ] Create command/query file
  - [ ] Create handler file
  - [ ] Create validator file (if needed)
  - [ ] Create endpoint file
  - [ ] Add endpoint mapping in module

#### 4. DbContext
- [ ] Add DbSet property
- [ ] Create EF Core configuration (if complex)
- [ ] Update DbInitializer for seed data

#### 5. Testing
- [ ] Add architecture tests
- [ ] Add unit tests for domain logic
- [ ] Add integration tests for endpoints

## Common Patterns

### Aggregate Relationships
```csharp
// One-to-Many
public class Loan : AuditableEntity
{
    public Guid MemberId { get; private set; }
    public Member Member { get; private set; } = default!;
    
    private readonly List<LoanRepayment> _repayments = new();
    public IReadOnlyCollection<LoanRepayment> Repayments => _repayments.AsReadOnly();
    
    public void AddRepayment(LoanRepayment repayment)
    {
        _repayments.Add(repayment);
    }
}
```

### Value Objects
```csharp
public record Money(decimal Amount, string Currency)
{
    public static Money operator +(Money left, Money right)
    {
        if (left.Currency != right.Currency)
            throw new InvalidOperationException("Cannot add money with different currencies");
        return new Money(left.Amount + right.Amount, left.Currency);
    }
}
```

### Domain Events
```csharp
public record LoanDisbursedEvent(Guid LoanId, decimal Amount, DateTimeOffset DisburseDate) : DomainEvent;

// In handler:
public class LoanDisbursedEventHandler : INotificationHandler<LoanDisbursedEvent>
{
    public Task Handle(LoanDisbursedEvent notification, CancellationToken ct)
    {
        // Update accounting, send notifications, etc.
        return Task.CompletedTask;
    }
}
```

## Entity Priority Order

### Must Have (Phase 1) - Core Operations
1. Member
2. Branch
3. Staff  
4. LoanProduct
5. Loan
6. SavingsProduct
7. SavingsAccount

### Should Have (Phase 2) - Extended Loan Features
8. LoanApplication
9. LoanSchedule
10. LoanRepayment
11. LoanCollateral
12. LoanGuarantor
13. LoanDisbursementTranche
14. InterestRateChange

### Could Have (Phase 3) - Risk & Compliance
15. KycDocument
16. AmlAlert
17. CreditScore
18. RiskAlert
19. CollectionCase
20. CollectionAction

### Nice to Have (Phase 4+) - Advanced Features
21. MobileWallet
22. InsurancePolicy
23. ShareAccount
24. InvestmentAccount
25. (... remaining entities)

## Notes

- **Keep domain logic in entities** - Don't make anemic models
- **Use domain events** for cross-aggregate communication
- **Validate in validators** - Keep handlers thin
- **Use specifications** for complex queries
- **Multi-tenancy** is handled automatically by HeroDbContext
- **Audit fields** are populated automatically
- **Transactions** are handled by MediatR pipeline behaviors

## References

- Todo module: `/src/Modules/Todo/`
- Identity module: `/src/Modules/Identity/`
- Architecture guide: `/src/ARCHITECTURE_GUIDE.md`
- Module templates: `/src/MODULE_TEMPLATES.md`
