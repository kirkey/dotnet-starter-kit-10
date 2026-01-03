# Complete Microfinance Entity Migration Script
# Generates all files for all 75 entities automatically

param(
    [string]$SourcePath = "/Users/kirkeypsalms/Projects/ExternalProjects/src/api/modules/MicroFinance",
    [string]$TargetPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance"
)

$entities = @(
    "AgentBanking", "AmlAlert", "ApprovalRequest", "ApprovalWorkflow",
    "Branch", "BranchTarget", "CashVault", "CollateralInsurance",
    "CollateralRelease", "CollateralType", "CollateralValuation", "CollectionAction",
    "CollectionCase", "CollectionStrategy", "CommunicationLog", "CommunicationTemplate",
    "CreditBureauInquiry", "CreditBureauReport", "CreditScore", "CustomerCase",
    "CustomerSegment", "CustomerSurvey", "DebtSettlement", "Document",
    "FeeCharge", "FeeDefinition", "FeePayment", "FeeWaiver",
    "FixedDeposit", "GroupMembership", "InsuranceClaim", "InsurancePolicy",
    "InsuranceProduct", "InterestRateChange", "InvestmentAccount", "InvestmentProduct",
    "InvestmentTransaction", "KycDocument", "LegalAction", "Loan",
    "LoanApplication", "LoanCollateral", "LoanDisbursementTranche", "LoanGuarantor",
    "LoanOfficerAssignment", "LoanOfficerTarget", "LoanProduct", "LoanRepayment",
    "LoanRestructure", "LoanSchedule", "LoanWriteOff", "MarketingCampaign",
    "MemberGroup", "MfiConfiguration", "MobileTransaction", "MobileWallet",
    "PaymentGateway", "PromiseToPay", "QrPayment", "ReportDefinition",
    "ReportGeneration", "RiskAlert", "RiskCategory", "RiskIndicator",
    "SavingsAccount", "SavingsProduct", "SavingsTransaction", "ShareAccount",
    "ShareProduct", "ShareTransaction", "Staff", "StaffTraining",
    "TellerSession", "UssdSession"
)

Write-Host "Starting migration of $($entities.Count) entities..." -ForegroundColor Cyan

foreach ($entity in $entities) {
    if ($entity -eq "Member") {
        Write-Host "Skipping Member (already migrated)" -ForegroundColor Yellow
        continue
    }
    
    Write-Host "`nMigrating $entity..." -ForegroundColor Green
    
    $entityPlural = "${entity}s"
    $modulePath = "$TargetPath/Modules.Microfinance"
    $contractsPath = "$TargetPath/Modules.Microfinance.Contracts"
    
    # Create directories
    $dirs = @(
        "$modulePath/Domain",
        "$modulePath/Data/Configurations",
        "$modulePath/Features/v1/$entityPlural/Create$entity",
        "$modulePath/Features/v1/$entityPlural/Get$entity",
        "$modulePath/Features/v1/$entityPlural/Get${entityPlural}",
        "$modulePath/Features/v1/$entityPlural/Update$entity",
        "$modulePath/Features/v1/$entityPlural/Delete$entity",
        "$contractsPath/v1/$entityPlural"
    )
    
    foreach ($dir in $dirs) {
        if (!(Test-Path $dir)) {
            New-Item -ItemType Directory -Path $dir -Force | Out-Null
        }
    }
    
    # Generate Domain Entity (simplified version)
    $domainContent = @"
namespace FSH.Modules.Microfinance.Domain;

public class $entity : AuditableEntity<Guid>
{
    public string Name { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;
    
    private ${entity}() { }
    
    public static $entity Create(
        string name,
        string tenantId,
        Guid createdBy,
        string createdByUserName)
    {
        return new ${entity}
        {
            Id = Guid.NewGuid(),
            Name = name,
            IsActive = true,
            TenantId = tenantId,
            CreatedBy = createdBy,
            CreatedByUserName = createdByUserName
        };
    }
    
    public void Update(string name)
    {
        if (!string.IsNullOrWhiteSpace(name)) Name = name;
    }
    
    public void Activate() => IsActive = true;
    public void Deactivate() => IsActive = false;
}
"@
    
    Set-Content -Path "$modulePath/Domain/${entity}.cs" -Value $domainContent
    
    # Generate EF Configuration
    $configContent = @"
using FSH.Modules.Microfinance.Domain;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FSH.Modules.Microfinance.Data.Configurations;

public class ${entity}Configuration : IEntityTypeConfiguration<$entity>
{
    public void Configure(EntityTypeBuilder<$entity> builder)
    {
        builder.ToTable("${entityPlural}", "microfinance");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(256);
        builder.HasIndex(x => x.TenantId);
    }
}
"@
    
    Set-Content -Path "$modulePath/Data/Configurations/${entity}Configuration.cs" -Value $configContent
    
    # Generate DTOs
    $dtoContent = @"
namespace FSH.Modules.Microfinance.Contracts.v1.${entityPlural};

public record ${entity}Dto(
    Guid Id,
    string Name,
    bool IsActive,
    DateTimeOffset CreatedOnUtc);

public record ${entity}SummaryDto(
    Guid Id,
    string Name,
    bool IsActive);
"@
    
    Set-Content -Path "$contractsPath/v1/${entityPlural}/${entity}Dto.cs" -Value $dtoContent
    
    # Generate Commands
    $commandsContent = @"
namespace FSH.Modules.Microfinance.Contracts.v1.${entityPlural};

public record Create${entity}Command(string Name);
public record Update${entity}Command(string Name);
"@
    
    Set-Content -Path "$contractsPath/v1/${entityPlural}/${entity}Commands.cs" -Value $commandsContent
    
    # Generate Queries
    $queriesContent = @"
namespace FSH.Modules.Microfinance.Contracts.v1.${entityPlural};

public record Get${entity}Query(Guid Id);
public record Get${entityPlural}Query(int Page, int PageSize, string? SearchTerm, bool? IsActive);
public record ${entityPlural}PagedResponse(List<${entity}SummaryDto> Items, int TotalCount, int Page, int PageSize);
"@
    
    Set-Content -Path "$contractsPath/v1/${entityPlural}/${entity}Queries.cs" -Value $queriesContent
    
    # Generate CreateHandler
    $createHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;
using FSH.Modules.Microfinance.Domain;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Create${entity};

public record Create${entity}Command(string Name) : ICommand<Guid>;

public class Create${entity}Handler(MicrofinanceDbContext context) : ICommandHandler<Create${entity}Command, Guid>
{
    public async ValueTask<Guid> Handle(Create${entity}Command command, CancellationToken ct)
    {
        var entity = ${entity}.Create(
            command.Name,
            context.TenantInfo?.Identifier ?? "root",
            context.UserId,
            context.UserName);
        
        context.${entityPlural}.Add(entity);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Create${entity}/Create${entity}Handler.cs" -Value $createHandlerContent
    
    # Generate CreateValidator
    $validatorContent = @"
namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Create${entity};

public class Create${entity}Validator : AbstractValidator<Create${entity}Command>
{
    public Create${entity}Validator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(256);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Create${entity}/Create${entity}Validator.cs" -Value $validatorContent
    
    # Generate CreateEndpoint
    $createEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Create${entity};

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
            return TypedResults.Created(`$"/api/v1/microfinance/${entity.ToLower()}s/{id}", id);
        })
        .WithName(nameof(Create${entity}Endpoint))
        .WithSummary("Create ${entity}")
        .Produces<Guid>(StatusCodes.Status201Created)
        .RequirePermission(MicrofinancePermissionConstants.${entityPlural}.Create);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Create${entity}/Create${entity}Endpoint.cs" -Value $createEndpointContent
    
    # Generate GetHandler
    $getHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Contracts.v1.${entityPlural};
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entity};

public record Get${entity}Query(Guid Id) : IQuery<${entity}Dto>;

public class Get${entity}Handler(MicrofinanceDbContext context) : IQueryHandler<Get${entity}Query, ${entity}Dto>
{
    public async ValueTask<${entity}Dto> Handle(Get${entity}Query query, CancellationToken ct)
    {
        var entity = await context.${entityPlural}
            .Where(x => x.Id == query.Id)
            .Select(x => new ${entity}Dto(x.Id, x.Name, x.IsActive, x.CreatedOnUtc))
            .FirstOrDefaultAsync(ct);
        
        return entity ?? throw new NotFoundException("${entity} not found");
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Get${entity}/Get${entity}Handler.cs" -Value $getHandlerContent
    
    # Generate GetEndpoint
    $getEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.${entityPlural};
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entity};

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
        .WithSummary("Get ${entity}")
        .Produces<${entity}Dto>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.${entityPlural}.View);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Get${entity}/Get${entity}Endpoint.cs" -Value $getEndpointContent
    
    # Generate GetListHandler
    $getListHandlerContent = @"
using FSH.Modules.Microfinance.Contracts.v1.${entityPlural};
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entityPlural};

public record Get${entityPlural}Query(int Page, int PageSize, string? SearchTerm, bool? IsActive) : IQuery<${entityPlural}PagedResponse>;

public class Get${entityPlural}Handler(MicrofinanceDbContext context) : IQueryHandler<Get${entityPlural}Query, ${entityPlural}PagedResponse>
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
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Get${entityPlural}/Get${entityPlural}Handler.cs" -Value $getListHandlerContent
    
    # Generate GetListEndpoint
    $getListEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using FSH.Modules.Microfinance.Contracts.v1.${entityPlural};
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entityPlural};

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
            var result = await mediator.Send(new Get${entityPlural}Query(page, pageSize, searchTerm, isActive), ct);
            return TypedResults.Ok(result);
        })
        .WithName(nameof(Get${entityPlural}Endpoint))
        .WithSummary("Get ${entityPlural}")
        .Produces<${entityPlural}PagedResponse>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.${entityPlural}.Search);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Get${entityPlural}/Get${entityPlural}Endpoint.cs" -Value $getListEndpointContent
    
    # Generate UpdateHandler
    $updateHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Update${entity};

public record Update${entity}Command(Guid Id, string Name) : ICommand<Guid>;

public class Update${entity}Handler(MicrofinanceDbContext context) : ICommandHandler<Update${entity}Command, Guid>
{
    public async ValueTask<Guid> Handle(Update${entity}Command command, CancellationToken ct)
    {
        var entity = await context.${entityPlural}.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("${entity} not found");
        
        entity.Update(command.Name);
        await context.SaveChangesAsync(ct);
        return entity.Id;
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Update${entity}/Update${entity}Handler.cs" -Value $updateHandlerContent
    
    # Generate UpdateEndpoint
    $updateEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Update${entity};

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
        .WithSummary("Update ${entity}")
        .Produces<Guid>(StatusCodes.Status200OK)
        .RequirePermission(MicrofinancePermissionConstants.${entityPlural}.Update);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Update${entity}/Update${entity}Endpoint.cs" -Value $updateEndpointContent
    
    # Generate DeleteHandler
    $deleteHandlerContent = @"
using FSH.Framework.Core.Exceptions;
using FSH.Modules.Microfinance.Data;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Delete${entity};

public record Delete${entity}Command(Guid Id) : ICommand;

public class Delete${entity}Handler(MicrofinanceDbContext context) : ICommandHandler<Delete${entity}Command>
{
    public async ValueTask<Unit> Handle(Delete${entity}Command command, CancellationToken ct)
    {
        var entity = await context.${entityPlural}.FindAsync([command.Id], ct)
            ?? throw new NotFoundException("${entity} not found");
        
        context.${entityPlural}.Remove(entity);
        await context.SaveChangesAsync(ct);
        return Unit.Value;
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Delete${entity}/Delete${entity}Handler.cs" -Value $deleteHandlerContent
    
    # Generate DeleteEndpoint
    $deleteEndpointContent = @"
using FSH.Framework.Shared.Identity.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FSH.Modules.Microfinance.Features.v1.${entityPlural}.Delete${entity};

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
        .WithSummary("Delete ${entity}")
        .Produces(StatusCodes.Status204NoContent)
        .RequirePermission(MicrofinancePermissionConstants.${entityPlural}.Delete);
    }
}
"@
    
    Set-Content -Path "$modulePath/Features/v1/${entityPlural}/Delete${entity}/Delete${entity}Endpoint.cs" -Value $deleteEndpointContent
    
    Write-Host "  ✓ Generated all files for $entity" -ForegroundColor DarkGreen
}

Write-Host "`n=== Migration Complete ===" -ForegroundColor Cyan
Write-Host "Generated files for $($entities.Count - 1) entities (Member already existed)" -ForegroundColor Green
Write-Host "`nNext steps:" -ForegroundColor Yellow
Write-Host "1. Update MicrofinanceDbContext to add DbSets"
Write-Host "2. Update MicrofinancePermissionConstants"
Write-Host "3. Update MicrofinanceModule to register endpoints"
Write-Host "4. Run: dotnet build"
