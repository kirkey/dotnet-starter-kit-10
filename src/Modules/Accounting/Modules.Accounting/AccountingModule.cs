using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Modules.Accounting.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

namespace FSH.Modules.Accounting;

/// <summary>
/// Accounting Module - A comprehensive module for managing accounting operations.
/// 
/// **Purpose:**
/// Provides a complete feature set for financial accounting management including:
/// - Chart of Accounts management and hierarchical organization
/// - General Ledger posting and trial balance calculations
/// - Accounts Payable and Accounts Receivable tracking
/// - Invoice, Bill, and Payment processing
/// - Bank reconciliation and account management
/// - Fixed asset depreciation and tracking
/// - Budget creation and variance analysis
/// - Journal entries and posting batch operations
/// - Regulatory reporting and compliance tracking
/// - Multi-tenant accounting period management
/// 
/// **Key Features:**
/// - Define and manage chart of accounts with USOA compliance
/// - Post transactions to general ledger with automatic balance updates
/// - Track vendor and customer accounts with aging analysis
/// - Process payments and allocate to invoices
/// - Perform bank reconciliations and account verification
/// - Manage fixed assets with depreciation methods
/// - Create and monitor budgets with variance tracking
/// - Generate trial balances and financial reports
/// - Support multi-company and intercompany transactions
/// - Maintain audit trail for all accounting operations
/// 
/// **Architecture:**
/// - Uses Entity Framework Core with PostgreSQL/MSSQL support
/// - Implements Domain-Driven Design with aggregate pattern
/// - Supports multi-tenancy through Finbuckle
/// - Uses CQRS pattern with Mediator library for commands/queries
/// - Includes FluentValidation for request validation
/// - Event-driven architecture for domain events
/// 
/// **Key Entities:**
/// - ChartOfAccount: Master account definitions in the chart of accounts
/// - GeneralLedger: Journal entries and posting records
/// - AccountsPayable: Vendor invoices and payment obligations
/// - AccountsReceivable: Customer invoices and receivables
/// - Invoice/Bill: Sales and purchase transaction documents
/// - Payment: Payment processing and allocation
/// - Bank: Bank account management
/// - FixedAsset: Asset tracking with depreciation
/// - Budget: Budget creation and monitoring
/// - JournalEntry: Manual accounting entries
/// - AccountingPeriod: Fiscal period management
/// - CostCenter: Cost allocation and tracking
/// 
/// **Permissions:**
/// - Accounting.View: View accounting data and reports
/// - Accounting.Search: Search accounting records
/// - Accounting.Create: Create new accounting transactions
/// - Accounting.Update: Modify existing accounting records
/// - Accounting.Delete: Delete accounting records
/// - Accounting.Post: Post transactions to general ledger
/// - Accounting.Reconcile: Perform bank and account reconciliations
/// - Accounting.Close: Close accounting periods
/// - Accounting.Report: Generate financial reports
/// </summary>
public class AccountingModule : IModule
{
    public void ConfigureServices(IHostApplicationBuilder builder)
    {
        // Register permissions
        PermissionConstants.Register(AccountingPermissionConstants.GetPermissions());

        // Register DbContext
        builder.Services.AddHeroDbContext<AccountingDbContext>();

        // Register Db Initializer
        builder.Services.AddScoped<IDbInitializer, AccountingDbInitializer>();

        // Health checks
        builder.Services.AddHealthChecks()
            .AddDbContextCheck<AccountingDbContext>(
                name: "db:accounting",
                failureStatus: HealthStatus.Unhealthy);
    }

    public void MapEndpoints(IEndpointRouteBuilder endpoints)
    {
        ApiVersionSet apiVersionSet = endpoints.NewApiVersionSet()
            .HasApiVersion(new ApiVersion(1))
            .ReportApiVersions()
            .Build();

        RouteGroupBuilder group = endpoints
            .MapGroup("api/v{version:apiVersion}/accounting")
            .WithTags("Accounting")
            .WithApiVersionSet(apiVersionSet);

        // Chart of Accounts endpoints will be added here
        // General Ledger endpoints will be added here
        // AP/AR endpoints will be added here
        // Invoice/Bill endpoints will be added here
        // Payment endpoints will be added here
        // Bank/Reconciliation endpoints will be added here
        // Fixed Asset endpoints will be added here
        // Budget endpoints will be added here
        // Journal Entry endpoints will be added here
        // Reporting endpoints will be added here
    }
}
