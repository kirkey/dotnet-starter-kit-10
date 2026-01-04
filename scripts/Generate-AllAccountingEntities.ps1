# Accounting Module Complete Migration Script
# Generates all files for all 50 accounting entities from Clean Architecture to Vertical Slice Architecture
# Follows FSH Framework patterns established in Todo and Microfinance modules

param(
    [string]$TargetPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Accounting"
)

# Define all 50 accounting entities with their metadata
$entities = @(
    @{ Name = "ChartOfAccount"; Plural = "ChartOfAccounts"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "GeneralLedger"; Plural = "GeneralLedger"; HasApproval = $false; Operations = @("Get", "GetList", "Export") },
    @{ Name = "JournalEntry"; Plural = "JournalEntries"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Post", "Approve", "Reverse") },
    @{ Name = "JournalEntryLine"; Plural = "JournalEntryLines"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "AccountingPeriod"; Plural = "AccountingPeriods"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Close", "Reopen") },
    @{ Name = "FiscalPeriodClose"; Plural = "FiscalPeriodClose"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Initiate", "Complete", "Reverse") },
    @{ Name = "TrialBalance"; Plural = "TrialBalance"; HasApproval = $false; Operations = @("Get", "Generate", "Export") },
    @{ Name = "PostingBatch"; Plural = "PostingBatches"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Approve", "Reject", "Post") },
    @{ Name = "Budget"; Plural = "Budgets"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve") },
    @{ Name = "BudgetDetail"; Plural = "BudgetDetails"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "AccountsPayableAccount"; Plural = "AccountsPayable"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "AccountsReceivableAccount"; Plural = "AccountsReceivable"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Invoice"; Plural = "Invoices"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve", "Send") },
    @{ Name = "InvoiceLineItem"; Plural = "InvoiceLineItems"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Bill"; Plural = "Bills"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve") },
    @{ Name = "BillLineItem"; Plural = "BillLineItems"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "CreditMemo"; Plural = "CreditMemos"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve") },
    @{ Name = "DebitMemo"; Plural = "DebitMemos"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve") },
    @{ Name = "Bank"; Plural = "Banks"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "BankReconciliation"; Plural = "BankReconciliations"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Approve", "Export") },
    @{ Name = "Check"; Plural = "Checks"; HasApproval = $false; Operations = @("Issue", "Get", "GetList", "Void", "Clear", "StopPayment", "Print") },
    @{ Name = "Payment"; Plural = "Payments"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Approve") },
    @{ Name = "PaymentAllocation"; Plural = "PaymentAllocations"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "AccountReconciliation"; Plural = "AccountReconciliations"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Approve") },
    @{ Name = "Payee"; Plural = "Payees"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "SecurityDeposit"; Plural = "SecurityDeposits"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Refund") },
    @{ Name = "WriteOff"; Plural = "WriteOffs"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Approve", "Reverse") },
    @{ Name = "FixedAsset"; Plural = "FixedAssets"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Depreciate", "Dispose") },
    @{ Name = "DepreciationMethod"; Plural = "DepreciationMethods"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "PrepaidExpense"; Plural = "PrepaidExpenses"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Amortize") },
    @{ Name = "Accrual"; Plural = "Accruals"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "DeferredRevenue"; Plural = "DeferredRevenue"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Recognize") },
    @{ Name = "InventoryItem"; Plural = "InventoryItems"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "AddStock", "ReduceStock") },
    @{ Name = "CostCenter"; Plural = "CostCenters"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Customer"; Plural = "Customers"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Member"; Plural = "Members"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Meter"; Plural = "Meters"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "Consumption"; Plural = "Consumption"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "PatronageCapital"; Plural = "PatronageCapital"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Allocate") },
    @{ Name = "InterconnectionAgreement"; Plural = "InterconnectionAgreements"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "PowerPurchaseAgreement"; Plural = "PowerPurchaseAgreements"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "RateSchedule"; Plural = "RateSchedules"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "RegulatoryReport"; Plural = "RegulatoryReports"; HasApproval = $false; Operations = @("Get", "GetList", "Generate", "Submit", "Export") },
    @{ Name = "Project"; Plural = "Projects"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "ProjectCost"; Plural = "ProjectCosts"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "InterCompanyTransaction"; Plural = "InterCompanyTransactions"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Reconcile") },
    @{ Name = "TaxCode"; Plural = "TaxCodes"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "RecurringJournalEntry"; Plural = "RecurringJournalEntries"; HasApproval = $true; Operations = @("Create", "Get", "GetList", "Update", "Delete", "Generate", "Approve") },
    @{ Name = "Vendor"; Plural = "Vendors"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Update", "Delete") },
    @{ Name = "RetainedEarnings"; Plural = "RetainedEarnings"; HasApproval = $false; Operations = @("Create", "Get", "GetList", "Close", "Reopen") }
)

Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Accounting Module VSA Migration" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Starting migration of $($entities.Count) entities..." -ForegroundColor Yellow
Write-Host ""

$totalOperations = 0
foreach ($entity in $entities) {
    $totalOperations += $entity.Operations.Count
}
Write-Host "Total operations to generate: $totalOperations" -ForegroundColor Yellow
Write-Host ""

$fileCount = 0
$operationCount = 0

foreach ($entityData in $entities) {
    $entity = $entityData.Name
    $entityPlural = $entityData.Plural
    $operations = $entityData.Operations
    
    Write-Host "Migrating $entity ($($operations.Count) operations)..." -ForegroundColor Green
    
    $modulePath = "$TargetPath/Modules.Accounting"
    $contractsPath = "$TargetPath/Modules.Accounting.Contracts"
    
    # Create Domain directory
    $domainDir = "$modulePath/Domain"
    if (!(Test-Path $domainDir)) {
        New-Item -ItemType Directory -Path $domainDir -Force | Out-Null
    }
    
    # Create Data/Configurations directory
    $configDir = "$modulePath/Data/Configurations"
    if (!(Test-Path $configDir)) {
        New-Item -ItemType Directory -Path $configDir -Force | Out-Null
    }
    
    # Create Contracts directory
    $contractsDir = "$contractsPath/v1/$entityPlural"
    if (!(Test-Path $contractsDir)) {
        New-Item -ItemType Directory -Path $contractsDir -Force | Out-Null
    }
    
    # Generate Domain Entity (simplified - will need manual enhancement)
    $domainContent = @"
namespace FSH.Modules.Accounting.Domain;

/// <summary>
/// Represents a $entity in the accounting system.
/// </summary>
public class $entity : AuditableEntity<Guid>, IMustHaveTenant
{
    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public bool IsActive { get; private set; } = true;
    public string TenantId { get; private set; } = default!;
    
    private ${entity}() { }
    
    public static $entity Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName,
        string? description = null)
    {
        return new ${entity}
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName,
            CreatedOnUtc = DateTimeOffset.UtcNow
        };
    }
    
    public void Update(string name, string? description = null)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
        Description = description;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
"@
    
    Set-Content -Path "$modulePath/Domain/${entity}.cs" -Value $domainContent
    $fileCount++
    
    # Generate EF Configuration
    $configContent = @"
using FSH.Modules.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Accounting.Data.Configurations;

public class ${entity}Configuration : IEntityTypeConfiguration<$entity>
{
    public void Configure(EntityTypeBuilder<$entity> builder)
    {
        builder.ToTable("${entityPlural}", "accounting");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(AccountingStringLengths.Name);
        builder.Property(x => x.Description).HasMaxLength(AccountingStringLengths.Description);
        builder.Property(x => x.TenantId).IsRequired().HasMaxLength(AccountingStringLengths.TenantId);
        builder.HasIndex(x => x.TenantId);
        builder.HasIndex(x => new { x.TenantId, x.Name });
    }
}
"@
    
    Set-Content -Path "$modulePath/Data/Configurations/${entity}Configuration.cs" -Value $configContent
    $fileCount++
    
    # Generate DTOs
    $dtoContent = @"
namespace FSH.Modules.Accounting.Contracts.v1.${entityPlural};

public record ${entity}Dto(
    Guid Id,
    string Name,
    string? Description,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ${entity}SummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
"@
    
    Set-Content -Path "$contractsPath/v1/${entityPlural}/${entity}Dto.cs" -Value $dtoContent
    $fileCount++
    
    # Generate each operation
    foreach ($operation in $operations) {
        $operationCount++
        
        # Create feature directory
        $featureDir = "$modulePath/Features/v1/$entityPlural/$operation$entity"
        if (!(Test-Path $featureDir)) {
            New-Item -ItemType Directory -Path $featureDir -Force | Out-Null
        }
        
        # Determine operation type and generate appropriate files
        switch ($operation) {
            "Create" {
                # Generate Create Command Handler
                $createHandlerContent = @"
using FSH.Framework.Core.Identity;
using FSH.Modules.Accounting.Data;
using FSH.Modules.Accounting.Domain;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Create${entity};

public record Create${entity}Command(string Name, string? Description) : ICommand<Guid>;

public class Create${entity}Handler(AccountingDbContext context, ICurrentUser currentUser) 
    : ICommandHandler<Create${entity}Command, Guid>
{
    public async ValueTask<Guid> Handle(Create${entity}Command command, CancellationToken ct)
    {
        var entity = ${entity}.Create(
            command.Name,
            currentUser.GetTenant() ?? "root",
            currentUser.GetUserId(),
            currentUser.Name ?? "System",
            command.Description);
        
        context.${entityPlural}.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
"@
                
                Set-Content -Path "$featureDir/Create${entity}Handler.cs" -Value $createHandlerContent
                $fileCount++
                
                # Generate Validator
                $validatorContent = @"
using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Create${entity};

public class Create${entity}Validator : AbstractValidator<Create${entity}Command>
{
    public Create${entity}Validator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
"@
                
                Set-Content -Path "$featureDir/Create${entity}Validator.cs" -Value $validatorContent
                $fileCount++
                
                # Generate Endpoint
                $createEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Create${entity};

public static class Create${entity}Endpoint
{
    public static RouteHandlerBuilder MapCreate${entity}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/", async (
            Create${entity}Command command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var id = await mediator.Send(command, ct);
            return TypedResults.Created(`$"/api/v1/accounting/${entityPlural.ToLower()}/{id}", id);
        })
        .WithName(nameof(Create${entity}Endpoint))
        .WithSummary("Create $entity")
        .Produces<Guid>(StatusCodes.Status201Created)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.Create);
    }
}
"@
                
                Set-Content -Path "$featureDir/Create${entity}Endpoint.cs" -Value $createEndpointContent
                $fileCount++
            }
            
            "Get" {
                # Generate Get Query Handler
                $getHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Contracts.v1.${entityPlural};
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Get${entity};

public record Get${entity}Query(Guid Id) : IQuery<${entity}Dto>;

public class Get${entity}Handler(AccountingDbContext context) : IQueryHandler<Get${entity}Query, ${entity}Dto>
{
    public async ValueTask<${entity}Dto> Handle(Get${entity}Query query, CancellationToken ct)
    {
        var entity = await context.${entityPlural}
            .Where(x => x.Id == query.Id)
            .Select(x => new ${entity}Dto(
                x.Id,
                x.Name,
                x.Description,
                x.IsActive,
                x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("$entity not found");
    }
}
"@
                
                Set-Content -Path "$featureDir/Get${entity}Handler.cs" -Value $getHandlerContent
                $fileCount++
                
                # Generate Get Endpoint
                $getEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.${entityPlural};
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Get${entity};

public static class Get${entity}Endpoint
{
    public static RouteHandlerBuilder MapGet${entity}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(new Get${entity}Query(id), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(Get${entity}Endpoint))
        .WithSummary("Get $entity by ID")
        .Produces<${entity}Dto>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.View);
    }
}
"@
                
                Set-Content -Path "$featureDir/Get${entity}Endpoint.cs" -Value $getEndpointContent
                $fileCount++
            }
            
            "GetList" {
                # Generate GetList Query Handler
                $getListHandlerContent = @"
using FSH.Modules.Accounting.Contracts.v1.${entityPlural};
using FSH.Modules.Accounting.Data;
using Mediator;
using Microsoft.EntityFrameworkCore;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Get${entityPlural};

public record Get${entityPlural}Query(
    int Page = 1,
    int PageSize = 10,
    string? SearchTerm = null,
    bool? IsActive = null) : IQuery<${entityPlural}PagedResponse>;

public record ${entityPlural}PagedResponse(
    List<${entity}SummaryDto> Items,
    int TotalCount,
    int Page,
    int PageSize);

public class Get${entityPlural}Handler(AccountingDbContext context) 
    : IQueryHandler<Get${entityPlural}Query, ${entityPlural}PagedResponse>
{
    public async ValueTask<${entityPlural}PagedResponse> Handle(Get${entityPlural}Query query, CancellationToken ct)
    {
        var queryable = context.${entityPlural}.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            queryable = queryable.Where(x => x.Name.Contains(query.SearchTerm));
        }
        
        if (query.IsActive.HasValue)
        {
            queryable = queryable.Where(x => x.IsActive == query.IsActive.Value);
        }
        
        var totalCount = await queryable.CountAsync(ct);
        
        var items = await queryable
            .OrderByDescending(x => x.CreatedOnUtc)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(x => new ${entity}SummaryDto(x.Id, x.Name, x.IsActive))
            .ToListAsync(ct);
        
        return new ${entityPlural}PagedResponse(items, totalCount, query.Page, query.PageSize);
    }
}
"@
                
                Set-Content -Path "$featureDir/Get${entityPlural}Handler.cs" -Value $getListHandlerContent
                $fileCount++
                
                # Generate GetList Endpoint
                $getListEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Accounting.Contracts.v1.${entityPlural};
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Get${entityPlural};

public static class Get${entityPlural}Endpoint
{
    public static RouteHandlerBuilder MapGet${entityPlural}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGet("/", async (
            int page,
            int pageSize,
            string? searchTerm,
            bool? isActive,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var result = await mediator.Send(
                new Get${entityPlural}Query(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(Get${entityPlural}Endpoint))
        .WithSummary("Get paginated list of ${entityPlural}")
        .Produces<${entityPlural}PagedResponse>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.Search);
    }
}
"@
                
                Set-Content -Path "$featureDir/Get${entityPlural}Endpoint.cs" -Value $getListEndpointContent
                $fileCount++
            }
            
            "Update" {
                # Generate Update Command Handler
                $updateHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Update${entity};

public record Update${entity}Command(Guid Id, string Name, string? Description) : ICommand<Guid>;

public class Update${entity}Handler(AccountingDbContext context) : ICommandHandler<Update${entity}Command, Guid>
{
    public async ValueTask<Guid> Handle(Update${entity}Command command, CancellationToken ct)
    {
        var entity = await context.${entityPlural}.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("$entity not found");
        
        entity.Update(command.Name, command.Description);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
"@
                
                Set-Content -Path "$featureDir/Update${entity}Handler.cs" -Value $updateHandlerContent
                $fileCount++
                
                # Generate Update Validator
                $updateValidatorContent = @"
using FluentValidation;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Update${entity};

public class Update${entity}Validator : AbstractValidator<Update${entity}Command>
{
    public Update${entity}Validator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(AccountingStringLengths.Name);
            
        When(x => !string.IsNullOrEmpty(x.Description), () =>
        {
            RuleFor(x => x.Description)
                .MaximumLength(AccountingStringLengths.Description);
        });
    }
}
"@
                
                Set-Content -Path "$featureDir/Update${entity}Validator.cs" -Value $updateValidatorContent
                $fileCount++
                
                # Generate Update Endpoint
                $updateEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Update${entity};

public static class Update${entity}Endpoint
{
    public static RouteHandlerBuilder MapUpdate${entity}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPut("/{id:guid}", async (
            Guid id,
            Update${entity}Command command,
            IMediator mediator,
            CancellationToken ct) =>
        {
            var updated = command with { Id = id };
            var result = await mediator.Send(updated, ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(Update${entity}Endpoint))
        .WithSummary("Update $entity")
        .Produces<Guid>(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.Update);
    }
}
"@
                
                Set-Content -Path "$featureDir/Update${entity}Endpoint.cs" -Value $updateEndpointContent
                $fileCount++
            }
            
            "Delete" {
                # Generate Delete Command Handler
                $deleteHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Delete${entity};

public record Delete${entity}Command(Guid Id) : ICommand;

public class Delete${entity}Handler(AccountingDbContext context) : ICommandHandler<Delete${entity}Command>
{
    public async ValueTask<Unit> Handle(Delete${entity}Command command, CancellationToken ct)
    {
        var entity = await context.${entityPlural}.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("$entity not found");
        
        context.${entityPlural}.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
"@
                
                Set-Content -Path "$featureDir/Delete${entity}Handler.cs" -Value $deleteHandlerContent
                $fileCount++
                
                # Generate Delete Endpoint
                $deleteEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.Delete${entity};

public static class Delete${entity}Endpoint
{
    public static RouteHandlerBuilder MapDelete${entity}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapDelete("/{id:guid}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new Delete${entity}Command(id), ct);
            return TypedResults.NoContent();
        })
        .WithName(nameof(Delete${entity}Endpoint))
        .WithSummary("Delete $entity")
        .Produces(StatusCodes.Status204NoContent)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.Delete);
    }
}
"@
                
                Set-Content -Path "$featureDir/Delete${entity}Endpoint.cs" -Value $deleteEndpointContent
                $fileCount++
            }
            
            default {
                # For custom operations (Approve, Post, etc.), generate stub files
                Write-Host "  - Generating stub for custom operation: $operation" -ForegroundColor Yellow
                
                $stubHandlerContent = @"
// TODO: Implement $operation operation for $entity
using FSH.Modules.Accounting.Data;
using Mediator;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.${operation}${entity};

public record ${operation}${entity}Command(Guid Id) : ICommand;

public class ${operation}${entity}Handler(AccountingDbContext context) 
    : ICommandHandler<${operation}${entity}Command>
{
    public async ValueTask<Unit> Handle(${operation}${entity}Command command, CancellationToken ct)
    {
        // TODO: Implement $operation logic
        throw new NotImplementedException("$operation operation for $entity needs to be implemented");
    }
}
"@
                
                Set-Content -Path "$featureDir/${operation}${entity}Handler.cs" -Value $stubHandlerContent
                $fileCount++
                
                $stubEndpointContent = @"
// TODO: Implement $operation endpoint for $entity
using FSH.Framework.Shared.Identity.Authorization;
using Mediator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Accounting.Features.v1.${entityPlural}.${operation}${entity};

public static class ${operation}${entity}Endpoint
{
    public static RouteHandlerBuilder Map${operation}${entity}Endpoint(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapPost("/{id:guid}/${operation.ToLower()}", async (
            Guid id,
            IMediator mediator,
            CancellationToken ct) =>
        {
            await mediator.Send(new ${operation}${entity}Command(id), ct);
            return TypedResults.Ok();
        })
        .WithName(nameof(${operation}${entity}Endpoint))
        .WithSummary("$operation $entity")
        .Produces(StatusCodes.Status200OK)
        .ProducesValidationProblem()
        .RequirePermission(AccountingPermissionConstants.${entityPlural}.${operation});
    }
}
"@
                
                Set-Content -Path "$featureDir/${operation}${entity}Endpoint.cs" -Value $stubEndpointContent
                $fileCount++
            }
        }
    }
    
    Write-Host "  ✓ Completed $entity - Generated $($operations.Count) operations" -ForegroundColor DarkGreen
}

Write-Host ""
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "  Migration Complete!" -ForegroundColor Cyan
Write-Host "========================================" -ForegroundColor Cyan
Write-Host "Generated $fileCount files for $($entities.Count) entities" -ForegroundColor Green
Write-Host "Generated $operationCount operations" -ForegroundColor Green
Write-Host ""
Write-Host "Next Steps:" -ForegroundColor Yellow
Write-Host "1. Create AccountingDbContext with all DbSets" -ForegroundColor White
Write-Host "2. Create AccountingModule.cs to register all endpoints" -ForegroundColor White
Write-Host "3. Create GlobalUsings.cs for common imports" -ForegroundColor White
Write-Host "4. Review and enhance domain entities with proper business logic" -ForegroundColor White
Write-Host "5. Implement custom operations (Approve, Post, etc.)" -ForegroundColor White
Write-Host "6. Create database migrations" -ForegroundColor White
Write-Host "7. Run: dotnet build" -ForegroundColor White
Write-Host ""
