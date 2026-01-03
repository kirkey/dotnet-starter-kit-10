# Generate Module Registration Script
# This creates the MicrofinanceModule.cs with all endpoint registrations

$entities = @(
    "Member", "AgentBanking", "AmlAlert", "ApprovalRequest", "ApprovalWorkflow",
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

$outputPath = "/Users/kirkeypsalms/Projects/dotnet-starter-kit-10/src/Modules/Microfinance/Modules.Microfinance"

# Generate using statements
$usings = @"
using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Modules.Microfinance.Data;
"@

foreach ($entity in $entities) {
    $entityPlural = "${entity}s"
    $usings += "`nusing FSH.Modules.Microfinance.Features.v1.${entityPlural}.Create${entity};"
    $usings += "`nusing FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entity};"
    $usings += "`nusing FSH.Modules.Microfinance.Features.v1.${entityPlural}.Get${entityPlural};"
    $usings += "`nusing FSH.Modules.Microfinance.Features.v1.${entityPlural}.Update${entity};"
    $usings += "`nusing FSH.Modules.Microfinance.Features.v1.${entityPlural}.Delete${entity};"
}

$usings += @"

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
"@

# Generate endpoint mappings
$mappings = ""
foreach ($entity in $entities) {
    $entityPlural = "${entity}s"
    $entityLower = $entity.ToLower()
    $mappings += @"

        // $entityPlural endpoints
        RouteGroupBuilder ${entityLower}Group = endpoints
            .MapGroup("api/v{version:apiVersion}/microfinance/${entityLower}s")
            .WithTags("$entityPlural")
            .WithApiVersionSet(apiVersionSet);

        ${entityLower}Group.MapCreate${entity}Endpoint();
        ${entityLower}Group.MapGet${entityPlural}Endpoint();
        ${entityLower}Group.MapGet${entity}Endpoint();
        ${entityLower}Group.MapUpdate${entity}Endpoint();
        ${entityLower}Group.MapDelete${entity}Endpoint();
"@
}

$moduleContent = @"
$usings

namespace FSH.Modules.Microfinance;

public class MicrofinanceModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(MicrofinancePermissionConstants.GetPermissions());

        // Register DbContext
        builder.Services.AddDbContext<MicrofinanceDbContext>();

        // Register Db Initializer
        builder.Services.AddScoped<MicrofinanceDbInitializer>();

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
$mappings
    }
}
"@

Set-Content -Path "$outputPath/MicrofinanceModule.cs" -Value $moduleContent

Write-Host "✓ MicrofinanceModule.cs generated successfully!" -ForegroundColor Green
Write-Host "  File: $outputPath/MicrofinanceModule.cs"
