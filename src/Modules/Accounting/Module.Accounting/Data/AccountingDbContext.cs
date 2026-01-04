using Finbuckle.MultiTenant.Abstractions;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Multitenancy;
using FSH.Framework.Shared.Persistence;
using FSH.Module.Accounting.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace FSH.Module.Accounting.Data;

/// <summary>
/// Entity Framework Core DbContext for the Accounting module.
/// 
/// **Purpose:**
/// Manages all database operations for the accounting system including:
/// - Chart of Accounts and General Ledger
/// - Journal Entries and Transactions
/// - Accounts Payable and Receivable
/// - Banking and Reconciliation
/// - Fixed Assets and Inventory
/// - Budgeting and Reporting
/// - Utility-specific entities (Meters, Consumption, Patronage Capital)
/// 
/// **Features:**
/// - Multi-tenant database isolation through connection string per tenant
/// - Automatic application of entity configurations
/// - Support for PostgreSQL, MSSQL, or other EF Core supported databases
/// - Proper DbSet definitions for all 50 accounting entities
/// - User context awareness for audit trail
/// 
/// **Configuration:**
/// - Applies all entity configurations from the assembly
/// - Configures database provider based on DatabaseOptions settings
/// - Tenant-specific connection string from MultiTenantContext
/// - User context from ICurrentUser for audit fields
/// </summary>
public class AccountingDbContext : DbContext
{
    private readonly DatabaseOptions _settings;
    private readonly ICurrentUser _currentUser;
    private AppTenantInfo TenantInfo { get; set; }
    private readonly IHostEnvironment _environment;

    // Financial Core
    public DbSet<ChartOfAccount> ChartOfAccounts => Set<ChartOfAccount>();
    public DbSet<GeneralLedger> GeneralLedger => Set<GeneralLedger>();
    public DbSet<JournalEntry> JournalEntries => Set<JournalEntry>();
    public DbSet<JournalEntryLine> JournalEntryLines => Set<JournalEntryLine>();
    public DbSet<AccountingPeriod> AccountingPeriods => Set<AccountingPeriod>();
    public DbSet<FiscalPeriodClose> FiscalPeriodClose => Set<FiscalPeriodClose>();
    public DbSet<TrialBalance> TrialBalance => Set<TrialBalance>();
    public DbSet<PostingBatch> PostingBatches => Set<PostingBatch>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<BudgetDetail> BudgetDetails => Set<BudgetDetail>();

    // Accounts Payable/Receivable
    public DbSet<AccountsPayableAccount> AccountsPayable => Set<AccountsPayableAccount>();
    public DbSet<AccountsReceivableAccount> AccountsReceivable => Set<AccountsReceivableAccount>();
    public DbSet<Invoice> Invoices => Set<Invoice>();
    public DbSet<InvoiceLineItem> InvoiceLineItems => Set<InvoiceLineItem>();
    public DbSet<Bill> Bills => Set<Bill>();
    public DbSet<BillLineItem> BillLineItems => Set<BillLineItem>();
    public DbSet<CreditMemo> CreditMemos => Set<CreditMemo>();
    public DbSet<DebitMemo> DebitMemos => Set<DebitMemo>();

    // Banking & Payments
    public DbSet<Bank> Banks => Set<Bank>();
    public DbSet<BankReconciliation> BankReconciliations => Set<BankReconciliation>();
    public DbSet<Check> Checks => Set<Check>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<PaymentAllocation> PaymentAllocations => Set<PaymentAllocation>();
    public DbSet<AccountReconciliation> AccountReconciliations => Set<AccountReconciliation>();
    public DbSet<Payee> Payees => Set<Payee>();
    public DbSet<SecurityDeposit> SecurityDeposits => Set<SecurityDeposit>();
    public DbSet<WriteOff> WriteOffs => Set<WriteOff>();

    // Assets & Expenses
    public DbSet<FixedAsset> FixedAssets => Set<FixedAsset>();
    public DbSet<DepreciationMethod> DepreciationMethods => Set<DepreciationMethod>();
    public DbSet<PrepaidExpense> PrepaidExpenses => Set<PrepaidExpense>();
    public DbSet<Accrual> Accruals => Set<Accrual>();
    public DbSet<DeferredRevenue> DeferredRevenue => Set<DeferredRevenue>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<CostCenter> CostCenters => Set<CostCenter>();

    // Customers & Vendors
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Member> Members => Set<Member>();
    public DbSet<Vendor> Vendors => Set<Vendor>();

    // Utility-Specific
    public DbSet<Meter> Meters => Set<Meter>();
    public DbSet<Consumption> Consumption => Set<Consumption>();
    public DbSet<PatronageCapital> PatronageCapital => Set<PatronageCapital>();
    public DbSet<InterconnectionAgreement> InterconnectionAgreements => Set<InterconnectionAgreement>();
    public DbSet<PowerPurchaseAgreement> PowerPurchaseAgreements => Set<PowerPurchaseAgreement>();
    public DbSet<RateSchedule> RateSchedules => Set<RateSchedule>();
    public DbSet<RegulatoryReport> RegulatoryReports => Set<RegulatoryReport>();

    // Projects & Tax
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectCost> ProjectCosts => Set<ProjectCost>();
    public DbSet<InterCompanyTransaction> InterCompanyTransactions => Set<InterCompanyTransaction>();
    public DbSet<TaxCode> TaxCodes => Set<TaxCode>();
    public DbSet<RecurringJournalEntry> RecurringJournalEntries => Set<RecurringJournalEntry>();
    public DbSet<RetainedEarnings> RetainedEarnings => Set<RetainedEarnings>();

    // User context properties for audit trail
    public Guid UserId => _currentUser.GetUserId();
    public string UserName => _currentUser.Name ?? "System";

    /// <summary>
    /// Initializes a new instance of the AccountingDbContext.
    /// 
    /// Sets up the context with multi-tenant awareness, user context, and database configuration options.
    /// </summary>
    /// <param name="multiTenantContextAccessor">Provides access to current tenant information.</param>
    /// <param name="currentUser">Provides access to current user information for audit trail.</param>
    /// <param name="options">EF Core DbContext options.</param>
    /// <param name="settings">Database provider and configuration settings.</param>
    /// <param name="environment">The host environment (Development, Production, etc.).</param>
    /// <exception cref="ArgumentNullException">Thrown if required dependencies are null.</exception>
    public AccountingDbContext(
        IMultiTenantContextAccessor<AppTenantInfo> multiTenantContextAccessor,
        ICurrentUser currentUser,
        DbContextOptions<AccountingDbContext> options,
        IOptions<DatabaseOptions> settings,
        IHostEnvironment environment) : base(options)
    {
        ArgumentNullException.ThrowIfNull(multiTenantContextAccessor);
        ArgumentNullException.ThrowIfNull(currentUser);
        ArgumentNullException.ThrowIfNull(settings);

        _currentUser = currentUser;
        _environment = environment;
        _settings = settings.Value;
        TenantInfo = multiTenantContextAccessor.MultiTenantContext.TenantInfo!;
    }

    /// <summary>
    /// Configures the data model using Entity Framework model builder.
    /// 
    /// Applies all entity configurations found in this assembly,
    /// allowing centralized mapping configuration for all accounting entities.
    /// </summary>
    /// <param name="builder">The model builder used to configure the model.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(AccountingDbContext).Assembly);
        builder.HasDefaultSchema("accounting");
    }

    /// <summary>
    /// Configures the database context options.
    /// 
    /// Sets up the connection string and database provider based on tenant-specific settings.
    /// Supports PostgreSQL, MSSQL, and other EF Core providers.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the context.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(TenantInfo?.ConnectionString))
        {
            optionsBuilder.UseDatabase(_settings.Provider, TenantInfo.ConnectionString!);

            if (_environment.IsDevelopment())
            {
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
    }
}
