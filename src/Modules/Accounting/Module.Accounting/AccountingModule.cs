using Asp.Versioning;
using Asp.Versioning.Builder;
using FSH.Framework.Persistence;
using FSH.Framework.Shared.Identity;
using FSH.Framework.Web.Modules;
using FSH.Module.Accounting.Data;
using Accounting.Application.Reports.TrialBalance.v1.Services;
using Accounting.Application.Reports.GeneralLedger.v1.Services;
using Accounting.Application.Reports.BankReconciliation.v1.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;

// Import all endpoint namespaces
using FSH.Module.Accounting.Features.v1.ChartOfAccounts.CreateChartOfAccount;
using FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccount;
using FSH.Module.Accounting.Features.v1.ChartOfAccounts.GetChartOfAccounts;
using FSH.Module.Accounting.Features.v1.ChartOfAccounts.UpdateChartOfAccount;
using FSH.Module.Accounting.Features.v1.ChartOfAccounts.DeleteChartOfAccount;

// Consumption endpoints
using FSH.Module.Accounting.Features.v1.Consumption.CreateConsumption;
using FSH.Module.Accounting.Features.v1.Consumption.GetConsumption;
using FSH.Module.Accounting.Features.v1.Consumption.GetListConsumption;
using FSH.Module.Accounting.Features.v1.Consumption.UpdateConsumption;
using FSH.Module.Accounting.Features.v1.Consumption.DeleteConsumption;

// Accounting Periods
using FSH.Module.Accounting.Features.v1.AccountingPeriods.CreateAccountingPeriod;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.GetAccountingPeriod;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.GetAccountingPeriods;
using FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CreateFiscalPeriodClose;
using FSH.Module.Accounting.Features.v1.FiscalPeriodClose.GetFiscalPeriodClose;
using FSH.Module.Accounting.Features.v1.FiscalPeriodClose.InitiateFiscalPeriodClose;
using FSH.Module.Accounting.Features.v1.FiscalPeriodClose.CompleteFiscalPeriodClose;
using FSH.Module.Accounting.Features.v1.FiscalPeriodClose.ReverseFiscalPeriodClose;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.UpdateAccountingPeriod;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.DeleteAccountingPeriod;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.CloseAccountingPeriod;
using FSH.Module.Accounting.Features.v1.AccountingPeriods.ReopenAccountingPeriod;

// Trial Balance
using FSH.Module.Accounting.Features.v1.TrialBalance.GetTrialBalance;
using FSH.Module.Accounting.Features.v1.TrialBalance.GenerateTrialBalance;
using FSH.Module.Accounting.Features.v1.TrialBalance.ExportTrialBalance;

// PatronageCapital endpoints
using FSH.Module.Accounting.Features.v1.PatronageCapital.CreatePatronageCapital;
using FSH.Module.Accounting.Features.v1.PatronageCapital.GetPatronageCapital;
using FSH.Module.Accounting.Features.v1.PatronageCapital.GetListPatronageCapital;
using FSH.Module.Accounting.Features.v1.PatronageCapital.UpdatePatronageCapital;
using FSH.Module.Accounting.Features.v1.PatronageCapital.DeletePatronageCapital;
using FSH.Module.Accounting.Features.v1.PatronageCapital.AllocatePatronageCapital;

// FuelConsumption placeholders
using FSH.Module.Accounting.Features.v1.FuelConsumption.CreateFuelConsumption;
using FSH.Module.Accounting.Features.v1.FuelConsumption.GetFuelConsumption;
using FSH.Module.Accounting.Features.v1.FuelConsumption.GetListFuelConsumption;
using FSH.Module.Accounting.Features.v1.FuelConsumption.UpdateFuelConsumption;

// Power Purchase Agreements
using FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.CreatePowerPurchaseAgreement;
using FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreement;
using FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.GetPowerPurchaseAgreements;
using FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.UpdatePowerPurchaseAgreement;
using FSH.Module.Accounting.Features.v1.PowerPurchaseAgreements.DeletePowerPurchaseAgreement;

// Rate Schedules
using FSH.Module.Accounting.Features.v1.RateSchedules.CreateRateSchedule;
using FSH.Module.Accounting.Features.v1.RateSchedules.GetRateSchedule;
using FSH.Module.Accounting.Features.v1.RateSchedules.GetRateSchedules;
using FSH.Module.Accounting.Features.v1.RateSchedules.UpdateRateSchedule;
using FSH.Module.Accounting.Features.v1.RateSchedules.DeleteRateSchedule;

// Projects
using FSH.Module.Accounting.Features.v1.Projects.CreateProject;
using FSH.Module.Accounting.Features.v1.Projects.GetProject;
using FSH.Module.Accounting.Features.v1.Projects.GetProjects;
using FSH.Module.Accounting.Features.v1.Projects.UpdateProject;
using FSH.Module.Accounting.Features.v1.Projects.DeleteProject;

// Project Costs
using FSH.Module.Accounting.Features.v1.ProjectCosts.CreateProjectCost;
using FSH.Module.Accounting.Features.v1.ProjectCosts.GetProjectCost;
using FSH.Module.Accounting.Features.v1.ProjectCosts.GetProjectCosts;
using FSH.Module.Accounting.Features.v1.ProjectCosts.UpdateProjectCost;
using FSH.Module.Accounting.Features.v1.ProjectCosts.DeleteProjectCost;

// Accounts Receivable (missing endpoints)
using FSH.Module.Accounting.Features.v1.AccountsReceivable.CreateAccountsReceivableAccount;
using FSH.Module.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivableAccount;
using FSH.Module.Accounting.Features.v1.AccountsReceivable.GetAccountsReceivable;
using FSH.Module.Accounting.Features.v1.AccountsReceivable.UpdateAccountsReceivableAccount;
using FSH.Module.Accounting.Features.v1.AccountsReceivable.DeleteAccountsReceivableAccount;

// Bills (AP)
using FSH.Module.Accounting.Features.v1.Bills.CreateBill;
using FSH.Module.Accounting.Features.v1.Bills.GetBill;
using FSH.Module.Accounting.Features.v1.Bills.GetBills;
using FSH.Module.Accounting.Features.v1.Bills.UpdateBill;
using FSH.Module.Accounting.Features.v1.Bills.DeleteBill;
using FSH.Module.Accounting.Features.v1.Bills.ApproveBill;

// Banks & Reconciliation
using FSH.Module.Accounting.Features.v1.Banks.CreateBank;
using FSH.Module.Accounting.Features.v1.Banks.GetBank;
using FSH.Module.Accounting.Features.v1.Banks.GetBanks;
using FSH.Module.Accounting.Features.v1.Banks.UpdateBank;
using FSH.Module.Accounting.Features.v1.Banks.DeleteBank;
using FSH.Module.Accounting.Features.v1.BankReconciliations.CreateBankReconciliation;
using FSH.Module.Accounting.Features.v1.BankReconciliations.GetBankReconciliation;
using FSH.Module.Accounting.Features.v1.BankReconciliations.GetBankReconciliations;

// Checks
using FSH.Module.Accounting.Features.v1.Checks.CreateCheck;
using FSH.Module.Accounting.Features.v1.Checks.GetCheck;
using FSH.Module.Accounting.Features.v1.Checks.GetChecks;
using FSH.Module.Accounting.Features.v1.Checks.UpdateCheck;
using FSH.Module.Accounting.Features.v1.Checks.DeleteCheck;
using FSH.Module.Accounting.Features.v1.Checks.VoidCheck;
using FSH.Module.Accounting.Features.v1.Checks.ClearCheck;
using FSH.Module.Accounting.Features.v1.Checks.PrintCheck;
using FSH.Module.Accounting.Features.v1.Checks.StopPaymentCheck;

// Payments
using FSH.Module.Accounting.Features.v1.Payments.CreatePayment;
using FSH.Module.Accounting.Features.v1.Payments.GetPayment;
using FSH.Module.Accounting.Features.v1.Payments.GetPayments;
using FSH.Module.Accounting.Features.v1.Payments.UpdatePayment;
using FSH.Module.Accounting.Features.v1.Payments.DeletePayment;
using FSH.Module.Accounting.Features.v1.Payments.ApprovePayment;

// Payment Allocations
using FSH.Module.Accounting.Features.v1.PaymentAllocations.CreatePaymentAllocation;
using FSH.Module.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocation;
using FSH.Module.Accounting.Features.v1.PaymentAllocations.GetPaymentAllocations;
using FSH.Module.Accounting.Features.v1.PaymentAllocations.UpdatePaymentAllocation;
using FSH.Module.Accounting.Features.v1.PaymentAllocations.DeletePaymentAllocation;

// Retained Earnings
using FSH.Module.Accounting.Features.v1.RetainedEarnings.CreateRetainedEarnings;
using FSH.Module.Accounting.Features.v1.RetainedEarnings.GetRetainedEarnings;
using FSH.Module.Accounting.Features.v1.RetainedEarnings.CloseRetainedEarnings;
using FSH.Module.Accounting.Features.v1.RetainedEarnings.ReopenRetainedEarnings;
using FSH.Module.Accounting.Features.v1.FuelConsumption.DeleteFuelConsumption;

// JournalEntries endpoints
using FSH.Module.Accounting.Features.v1.JournalEntries.CreateJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.GetJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.GetListJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.UpdateJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.DeleteJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.PostJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.ApproveJournalEntry;
using FSH.Module.Accounting.Features.v1.JournalEntries.ReverseJournalEntry;

// JournalEntryLines endpoints
using FSH.Module.Accounting.Features.v1.JournalEntryLines.CreateJournalEntryLine;
using FSH.Module.Accounting.Features.v1.JournalEntryLines.GetJournalEntryLine;
using FSH.Module.Accounting.Features.v1.JournalEntryLines.GetListJournalEntryLine;
using FSH.Module.Accounting.Features.v1.JournalEntryLines.UpdateJournalEntryLine;
using FSH.Module.Accounting.Features.v1.JournalEntryLines.DeleteJournalEntryLine;

// PostingBatches endpoints
using FSH.Module.Accounting.Features.v1.PostingBatches.CreatePostingBatch;
using FSH.Module.Accounting.Features.v1.PostingBatches.GetPostingBatch;
using FSH.Module.Accounting.Features.v1.PostingBatches.GetListPostingBatch;
using FSH.Module.Accounting.Features.v1.PostingBatches.ApprovePostingBatch;
using FSH.Module.Accounting.Features.v1.PostingBatches.RejectPostingBatch;
using FSH.Module.Accounting.Features.v1.PostingBatches.PostPostingBatch;

// RecurringJournalEntries endpoints
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.CreateRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GetRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GetListRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.UpdateRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.DeleteRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.GenerateRecurringJournalEntry;
using FSH.Module.Accounting.Features.v1.RecurringJournalEntries.ApproveRecurringJournalEntry;

// FixedAssets endpoints
using FSH.Module.Accounting.Features.v1.FixedAssets.CreateFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.GetFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.GetListFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.UpdateFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.DeleteFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.DepreciateFixedAsset;
using FSH.Module.Accounting.Features.v1.FixedAssets.DisposeFixedAsset;

// TaxCodes endpoints
using FSH.Module.Accounting.Features.v1.TaxCodes.CreateTaxCode;
using FSH.Module.Accounting.Features.v1.TaxCodes.GetTaxCode;
using FSH.Module.Accounting.Features.v1.TaxCodes.GetListTaxCode;
using FSH.Module.Accounting.Features.v1.TaxCodes.UpdateTaxCode;
using FSH.Module.Accounting.Features.v1.TaxCodes.DeleteTaxCode;
// TODO: Import remaining endpoint namespaces for all 50 entities
// (Will be populated once all endpoints are reviewed and finalized)

namespace FSH.Module.Accounting;

/// <summary>
/// Accounting Module - Comprehensive accounting system for utility cooperatives.
/// 
/// **Purpose:**
/// Provides a complete accounting system with support for:
/// - USOA-compliant chart of accounts
/// - General ledger and journal entries
/// - Accounts payable and receivable
/// - Banking and reconciliation
/// - Fixed assets and inventory management
/// - Budgeting and financial reporting
/// - Utility-specific features (meters, consumption, patronage capital)
/// - Regulatory compliance and reporting
/// 
/// **Key Features:**
/// - 50 entity types covering all aspects of utility accounting
/// - Multi-tenant isolation with tenant-specific databases
/// - Full audit trail for compliance
/// - CQRS pattern with Mediator for commands/queries
/// - FluentValidation for all operations
/// - Approval workflows for critical transactions
/// - Bank and account reconciliation
/// - Period close and fiscal year management
/// 
/// **Architecture:**
/// - Vertical Slice Architecture with features organized by entity
/// - Entity Framework Core with PostgreSQL/MSSQL support
/// - Domain-Driven Design with aggregate patterns
/// - Multi-tenancy through Finbuckle
/// - Permission-based access control
/// 
/// **Major Entity Groups:**
/// 
/// **Financial Core (10):**
/// - ChartOfAccount, GeneralLedger, JournalEntry, JournalEntryLine
/// - AccountingPeriod, FiscalPeriodClose, TrialBalance, PostingBatch
/// - Budget, BudgetDetail
/// 
/// **AP/AR (8):**
/// - AccountsPayableAccount, AccountsReceivableAccount
/// - Invoice, InvoiceLineItem, Bill, BillLineItem
/// - CreditMemo, DebitMemo
/// 
/// **Banking (9):**
/// - Bank, BankReconciliation, Check, Payment, PaymentAllocation
/// - AccountReconciliation, Payee, SecurityDeposit, WriteOff
/// 
/// **Assets & Expenses (7):**
/// - FixedAsset, DepreciationMethod, PrepaidExpense, Accrual
/// - DeferredRevenue, InventoryItem, CostCenter
/// 
/// **Utility-Specific (10):**
/// - Customer, Member, Vendor, Meter, Consumption
/// - PatronageCapital, InterconnectionAgreement, PowerPurchaseAgreement
/// - RateSchedule, RegulatoryReport
/// 
/// **Projects & Tax (6):**
/// - Project, ProjectCost, InterCompanyTransaction
/// - TaxCode, RecurringJournalEntry, RetainedEarnings
/// **Permissions:**
/// ~250 permissions covering all operations across all entities
/// See AccountingPermissionConstants for complete list
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

        // Register report services
        builder.Services.AddScoped<ITrialBalanceReportService, TrialBalanceReportService>();
        builder.Services.AddScoped<IGeneralLedgerReportService, GeneralLedgerReportService>();
        builder.Services.AddScoped<IBankReconciliationReportService, BankReconciliationReportService>();

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

        // ========================================
        // Financial Core Endpoints
        // ========================================
        
        // Chart of Accounts
        RouteGroupBuilder chartOfAccountsGroup = group.MapGroup("/chartofaccounts");
        chartOfAccountsGroup.MapCreateChartOfAccountEndpoint();
        chartOfAccountsGroup.MapGetChartOfAccountEndpoint();
        chartOfAccountsGroup.MapGetChartOfAccountsEndpoint();
        chartOfAccountsGroup.MapUpdateChartOfAccountEndpoint();
        chartOfAccountsGroup.MapDeleteChartOfAccountEndpoint();

        // General Ledger
        RouteGroupBuilder glGroup = group.MapGroup("/generalledger");
        glGroup.MapGetGeneralLedgerEndpoint();
        glGroup.MapGetGeneralLedgersEndpoint();
        glGroup.MapExportGeneralLedgerEndpoint();

        // Journal Entries
        RouteGroupBuilder journalGroup = group.MapGroup("/journalentries");
        journalGroup.MapCreateJournalEntryEndpoint();
        journalGroup.MapGetJournalEntryEndpoint();
        journalGroup.MapGetJournalEntriesEndpoint();
        journalGroup.MapUpdateJournalEntryEndpoint();
        journalGroup.MapDeleteJournalEntryEndpoint();
        journalGroup.MapPostJournalEntryEndpoint();
        journalGroup.MapApproveJournalEntryEndpoint();
        journalGroup.MapReverseJournalEntryEndpoint();

        // Journal Entry Lines
        RouteGroupBuilder journalLinesGroup = group.MapGroup("/journalentrylines");
        journalLinesGroup.MapCreateJournalEntryLineEndpoint();
        journalLinesGroup.MapGetJournalEntryLineEndpoint();
        journalLinesGroup.MapGetJournalEntryLinesEndpoint();
        journalLinesGroup.MapUpdateJournalEntryLineEndpoint();
        journalLinesGroup.MapDeleteJournalEntryLineEndpoint();

        // Accounting Periods
        RouteGroupBuilder periodsGroup = group.MapGroup("/accountingperiods");
        periodsGroup.MapCreateAccountingPeriodEndpoint();
        periodsGroup.MapGetAccountingPeriodsEndpoint();
        periodsGroup.MapGetAccountingPeriodEndpoint();
        periodsGroup.MapUpdateAccountingPeriodEndpoint();
        periodsGroup.MapDeleteAccountingPeriodEndpoint();
        periodsGroup.MapCloseAccountingPeriodEndpoint();
        periodsGroup.MapReopenAccountingPeriodEndpoint();

        // Fiscal Period Close
        RouteGroupBuilder fiscalCloseGroup = group.MapGroup("/fiscalperiodclose");
        fiscalCloseGroup.MapCreateFiscalPeriodCloseEndpoint();
        fiscalCloseGroup.MapGetFiscalPeriodClosesEndpoint();
        fiscalCloseGroup.MapGetFiscalPeriodCloseEndpoint();
        fiscalCloseGroup.MapInitiateFiscalPeriodCloseEndpoint();
        fiscalCloseGroup.MapCompleteFiscalPeriodCloseEndpoint();
        fiscalCloseGroup.MapReverseFiscalPeriodCloseEndpoint();

        // Trial Balance
        RouteGroupBuilder trialBalanceGroup = group.MapGroup("/trialbalance");
        trialBalanceGroup.MapGetTrialBalanceEndpoint();
        trialBalanceGroup.MapGenerateTrialBalanceEndpoint();
        trialBalanceGroup.MapExportTrialBalanceEndpoint();

        // Posting Batches
        RouteGroupBuilder postingBatchGroup = group.MapGroup("/postingbatches");
        postingBatchGroup.MapCreatePostingBatchEndpoint();
        postingBatchGroup.MapGetPostingBatchEndpoint();
        postingBatchGroup.MapGetPostingBatchesEndpoint();
        postingBatchGroup.MapApprovePostingBatchEndpoint();
        postingBatchGroup.MapRejectPostingBatchEndpoint();
        postingBatchGroup.MapPostPostingBatchEndpoint();

        // Budgets
        RouteGroupBuilder budgetGroup = group.MapGroup("/budgets");
        budgetGroup.MapCreateBudgetEndpoint();
        budgetGroup.MapGetBudgetEndpoint();
        budgetGroup.MapGetBudgetsEndpoint();
        budgetGroup.MapUpdateBudgetEndpoint();
        budgetGroup.MapDeleteBudgetEndpoint();
        budgetGroup.MapApproveBudgetEndpoint();

        // Budget Details
        RouteGroupBuilder budgetDetailsGroup = group.MapGroup("/budgetdetails");
        budgetDetailsGroup.MapCreateBudgetDetailEndpoint();
        budgetDetailsGroup.MapGetBudgetDetailEndpoint();
        budgetDetailsGroup.MapGetBudgetDetailsEndpoint();
        budgetDetailsGroup.MapUpdateBudgetDetailEndpoint();
        budgetDetailsGroup.MapDeleteBudgetDetailEndpoint();

        // ========================================
        // AP/AR Endpoints
        // ========================================

        // Accounts Payable
        RouteGroupBuilder apGroup = group.MapGroup("/accountspayable");
        apGroup.MapCreateAccountsPayableAccountEndpoint();
        apGroup.MapGetAccountsPayableAccountEndpoint();
        apGroup.MapGetAccountsPayableEndpoint();
        apGroup.MapUpdateAccountsPayableAccountEndpoint();
        apGroup.MapDeleteAccountsPayableAccountEndpoint();

        // Accounts Receivable
        RouteGroupBuilder arGroup = group.MapGroup("/accountsreceivable");
        arGroup.MapCreateAccountsReceivableAccountEndpoint();
        arGroup.MapGetAccountsReceivableAccountEndpoint();
        arGroup.MapGetAccountsReceivableEndpoint();
        arGroup.MapUpdateAccountsReceivableAccountEndpoint();
        arGroup.MapDeleteAccountsReceivableAccountEndpoint();

        // Invoices
        // TODO: Map Invoice endpoints (Create, Get, GetList, Update, Delete, Approve, Send)
        // RouteGroupBuilder invoiceGroup = group.MapGroup("/invoices");

        // Invoice Line Items
        // TODO: Map InvoiceLineItem endpoints
        // RouteGroupBuilder invoiceLinesGroup = group.MapGroup("/invoicelineitems");

        // Bills
        RouteGroupBuilder billGroup = group.MapGroup("/bills");
        billGroup.MapCreateBillEndpoint();
        billGroup.MapGetBillEndpoint();
        billGroup.MapGetBillsEndpoint();
        billGroup.MapUpdateBillEndpoint();
        billGroup.MapDeleteBillEndpoint();
        billGroup.MapApproveBillEndpoint();

        // Bill Line Items
        // TODO: Map BillLineItem endpoints
        // RouteGroupBuilder billLinesGroup = group.MapGroup("/billlineitems");

        // Credit Memos
        // TODO: Map CreditMemo endpoints (Create, Get, GetList, Update, Delete, Approve)
        // RouteGroupBuilder creditMemoGroup = group.MapGroup("/creditmemos");

        // Debit Memos
        // TODO: Map DebitMemo endpoints (Create, Get, GetList, Update, Delete, Approve)
        // RouteGroupBuilder debitMemoGroup = group.MapGroup("/debitmemos");

        // ========================================
        // Banking & Payments Endpoints
        // ========================================

        // Banks
        RouteGroupBuilder bankGroup = group.MapGroup("/banks");
        bankGroup.MapCreateBankEndpoint();
        bankGroup.MapGetBankEndpoint();
        bankGroup.MapGetBanksEndpoint();
        bankGroup.MapUpdateBankEndpoint();
        bankGroup.MapDeleteBankEndpoint();

        // Bank Reconciliations
        RouteGroupBuilder bankReconGroup = group.MapGroup("/bankreconciliations");
        bankReconGroup.MapCreateBankReconciliationEndpoint();
        bankReconGroup.MapGetBankReconciliationEndpoint();
        bankReconGroup.MapGetBankReconciliationsEndpoint();
        bankReconGroup.MapApproveBankReconciliationEndpoint();
        bankReconGroup.MapExportBankReconciliationEndpoint();

        // Checks
        RouteGroupBuilder checkGroup = group.MapGroup("/checks");
        checkGroup.MapIssueCheckEndpoint();
        checkGroup.MapGetCheckEndpoint();
        checkGroup.MapGetChecksEndpoint();
        checkGroup.MapVoidCheckEndpoint();
        checkGroup.MapClearCheckEndpoint();
        checkGroup.MapStopPaymentCheckEndpoint();
        checkGroup.MapPrintCheckEndpoint();
        checkGroup.MapCreateCheckEndpoint();
        checkGroup.MapUpdateCheckEndpoint();
        checkGroup.MapDeleteCheckEndpoint();

        // Payments
        RouteGroupBuilder paymentGroup = group.MapGroup("/payments");
        paymentGroup.MapCreatePaymentEndpoint();
        paymentGroup.MapGetPaymentEndpoint();
        paymentGroup.MapGetPaymentsEndpoint();
        paymentGroup.MapUpdatePaymentEndpoint();
        paymentGroup.MapDeletePaymentEndpoint();
        paymentGroup.MapApprovePaymentEndpoint();

        // Payment Allocations
        RouteGroupBuilder paymentAllocationGroup = group.MapGroup("/paymentallocations");
        paymentAllocationGroup.MapCreatePaymentAllocationEndpoint();
        paymentAllocationGroup.MapGetPaymentAllocationEndpoint();
        paymentAllocationGroup.MapGetPaymentAllocationsEndpoint();
        paymentAllocationGroup.MapUpdatePaymentAllocationEndpoint();
        paymentAllocationGroup.MapDeletePaymentAllocationEndpoint();

        // Account Reconciliations
        // TODO: Map AccountReconciliation endpoints (Create, Get, GetList, Approve)
        // RouteGroupBuilder acctReconGroup = group.MapGroup("/accountreconciliations");

        // Payees
        // TODO: Map Payee endpoints (Create, Get, GetList, Update, Delete)
        // RouteGroupBuilder payeeGroup = group.MapGroup("/payees");

        // Security Deposits
        // TODO: Map SecurityDeposit endpoints (Create, Get, GetList, Update, Delete, Refund)
        // RouteGroupBuilder securityDepositGroup = group.MapGroup("/securitydeposits");

        // Write-Offs
        // TODO: Map WriteOff endpoints (Create, Get, GetList, Approve, Reverse)
        // RouteGroupBuilder writeOffGroup = group.MapGroup("/writeoffs");

        // ========================================
        // Assets & Expenses Endpoints
        // ========================================

        // Fixed Assets
        // Fixed Assets
        RouteGroupBuilder fixedAssetGroup = group.MapGroup("/fixedassets");
        fixedAssetGroup.MapCreateFixedAssetEndpoint();
        fixedAssetGroup.MapGetFixedAssetEndpoint();
        fixedAssetGroup.MapGetFixedAssetsEndpoint();
        fixedAssetGroup.MapUpdateFixedAssetEndpoint();
        fixedAssetGroup.MapDeleteFixedAssetEndpoint();
        fixedAssetGroup.MapDepreciateFixedAssetEndpoint();
        fixedAssetGroup.MapDisposeFixedAssetEndpoint();

        // Depreciation Methods
        // TODO: Map DepreciationMethod endpoints (Create, Get, GetList, Update, Delete)
        // RouteGroupBuilder depreciationGroup = group.MapGroup("/depreciationmethods");

        // Prepaid Expenses
        // TODO: Map PrepaidExpense endpoints (Create, Get, GetList, Update, Delete, Amortize)
        // RouteGroupBuilder prepaidGroup = group.MapGroup("/prepaidexpenses");

        // Accruals
        // TODO: Map Accrual endpoints (Create, Get, GetList, Update, Delete)
        // RouteGroupBuilder accrualGroup = group.MapGroup("/accruals");

        // Deferred Revenue
        // TODO: Map DeferredRevenue endpoints (Create, Get, GetList, Update, Delete, Recognize)
        // RouteGroupBuilder deferredRevenueGroup = group.MapGroup("/deferredrevenue");

        // Inventory Items
        // Inventory Items
        RouteGroupBuilder inventoryGroup = group.MapGroup("/inventoryitems");
        inventoryGroup.MapCreateInventoryItemEndpoint();
        inventoryGroup.MapGetInventoryItemEndpoint();
        inventoryGroup.MapGetInventoryItemsEndpoint();
        inventoryGroup.MapUpdateInventoryItemEndpoint();
        inventoryGroup.MapDeleteInventoryItemEndpoint();
        inventoryGroup.MapAddStockInventoryItemEndpoint();
        inventoryGroup.MapReduceStockInventoryItemEndpoint();
        // RouteGroupBuilder inventoryGroup = group.MapGroup("/inventoryitems");

        // Cost Centers
        RouteGroupBuilder costCenterGroup = group.MapGroup("/costcenters");
        costCenterGroup.MapCreateCostCenterEndpoint();
        costCenterGroup.MapGetCostCenterEndpoint();
        costCenterGroup.MapGetCostCentersEndpoint();
        costCenterGroup.MapUpdateCostCenterEndpoint();
        costCenterGroup.MapDeleteCostCenterEndpoint();

        // ========================================
        // Utility-Specific Endpoints
        // ========================================

        // Customers
        RouteGroupBuilder customerGroup = group.MapGroup("/customers");
        customerGroup.MapCreateCustomerEndpoint();
        customerGroup.MapGetCustomerEndpoint();
        customerGroup.MapGetCustomersEndpoint();
        customerGroup.MapUpdateCustomerEndpoint();
        customerGroup.MapDeleteCustomerEndpoint();

        // Members
        RouteGroupBuilder memberGroup = group.MapGroup("/members");
        memberGroup.MapCreateMemberEndpoint();
        memberGroup.MapGetMemberEndpoint();
        memberGroup.MapGetMembersEndpoint();
        memberGroup.MapUpdateMemberEndpoint();
        memberGroup.MapDeleteMemberEndpoint();

        // Vendors
        RouteGroupBuilder vendorGroup = group.MapGroup("/vendors");
        vendorGroup.MapCreateVendorEndpoint();
        vendorGroup.MapGetVendorEndpoint();
        vendorGroup.MapGetVendorsEndpoint();
        vendorGroup.MapUpdateVendorEndpoint();
        vendorGroup.MapDeleteVendorEndpoint();

        // Meters
        RouteGroupBuilder meterGroup = group.MapGroup("/meters");
        meterGroup.MapCreateMeterEndpoint();
        meterGroup.MapGetMeterEndpoint();
        meterGroup.MapGetMetersEndpoint();
        meterGroup.MapUpdateMeterEndpoint();
        meterGroup.MapDeleteMeterEndpoint();

        // Consumption
        RouteGroupBuilder consumptionGroup = group.MapGroup("/consumptions");
        consumptionGroup.MapCreateConsumptionEndpoint();
        consumptionGroup.MapGetConsumptionsEndpoint();
        consumptionGroup.MapGetConsumptionEndpoint();
        consumptionGroup.MapUpdateConsumptionEndpoint();
        consumptionGroup.MapDeleteConsumptionEndpoint();

        // Patronage Capital
        RouteGroupBuilder patronageGroup = group.MapGroup("/patronagecapital");
        patronageGroup.MapCreatePatronageCapitalEndpoint();
        patronageGroup.MapGetPatronageCapitalsEndpoint();
        patronageGroup.MapGetPatronageCapitalEndpoint();
        patronageGroup.MapUpdatePatronageCapitalEndpoint();
        patronageGroup.MapDeletePatronageCapitalEndpoint();
        patronageGroup.MapAllocatePatronageCapitalEndpoint();

        // Fuel Consumption (placeholder endpoints)
        RouteGroupBuilder fuelConsumptionGroup = group.MapGroup("/fuelconsumptions");
        fuelConsumptionGroup.MapCreateFuelConsumptionEndpoint();
        fuelConsumptionGroup.MapGetFuelConsumptionsEndpoint();
        fuelConsumptionGroup.MapGetFuelConsumptionEndpoint();
        fuelConsumptionGroup.MapUpdateFuelConsumptionEndpoint();
        fuelConsumptionGroup.MapDeleteFuelConsumptionEndpoint();

        // Interconnection Agreements
        RouteGroupBuilder interconnectionGroup = group.MapGroup("/interconnectionagreements");
        interconnectionGroup.MapCreateInterconnectionAgreementEndpoint();
        interconnectionGroup.MapGetInterconnectionAgreementEndpoint();
        interconnectionGroup.MapGetInterconnectionAgreementsEndpoint();
        interconnectionGroup.MapUpdateInterconnectionAgreementEndpoint();
        interconnectionGroup.MapDeleteInterconnectionAgreementEndpoint();

        // Power Purchase Agreements
        RouteGroupBuilder ppaGroup = group.MapGroup("/powerpurchaseagreements");
        ppaGroup.MapCreatePowerPurchaseAgreementEndpoint();
        ppaGroup.MapGetPowerPurchaseAgreementEndpoint();
        ppaGroup.MapGetPowerPurchaseAgreementsEndpoint();
        ppaGroup.MapUpdatePowerPurchaseAgreementEndpoint();
        ppaGroup.MapDeletePowerPurchaseAgreementEndpoint();

        // Rate Schedules
        // Rate Schedules
        RouteGroupBuilder rateScheduleGroup = group.MapGroup("/rateschedules");
        rateScheduleGroup.MapCreateRateScheduleEndpoint();
        rateScheduleGroup.MapGetRateScheduleEndpoint();
        rateScheduleGroup.MapGetRateSchedulesEndpoint();
        rateScheduleGroup.MapUpdateRateScheduleEndpoint();
        rateScheduleGroup.MapDeleteRateScheduleEndpoint();

        // Regulatory Reports
        // TODO: Map RegulatoryReport endpoints (Get, GetList, Generate, Submit, Export)
        // RouteGroupBuilder regulatoryGroup = group.MapGroup("/regulatoryreports");

        // ========================================
        // Projects & Tax Endpoints
        // ========================================

        // Projects
        // Projects
        RouteGroupBuilder projectGroup = group.MapGroup("/projects");
        projectGroup.MapCreateProjectEndpoint();
        projectGroup.MapGetProjectEndpoint();
        projectGroup.MapGetProjectsEndpoint();
        projectGroup.MapUpdateProjectEndpoint();
        projectGroup.MapDeleteProjectEndpoint();

        // Project Costs
        RouteGroupBuilder projectCostGroup = group.MapGroup("/projectcosts");
        projectCostGroup.MapCreateProjectCostEndpoint();
        projectCostGroup.MapGetProjectCostEndpoint();
        projectCostGroup.MapGetProjectCostsEndpoint();
        projectCostGroup.MapUpdateProjectCostEndpoint();
        projectCostGroup.MapDeleteProjectCostEndpoint();

        // InterCompany Transactions
        // TODO: Map InterCompanyTransaction endpoints (Create, Get, GetList, Update, Delete, Reconcile)
        // RouteGroupBuilder intercompanyGroup = group.MapGroup("/intercompanytransactions");

        // Tax Codes
        RouteGroupBuilder taxCodeGroup = group.MapGroup("/taxcodes");
        taxCodeGroup.MapCreateTaxCodeEndpoint();
        taxCodeGroup.MapGetTaxCodeEndpoint();
        taxCodeGroup.MapGetTaxCodesEndpoint();
        taxCodeGroup.MapUpdateTaxCodeEndpoint();
        taxCodeGroup.MapDeleteTaxCodeEndpoint();

        // Recurring Journal Entries
        // Recurring Journal Entries
        RouteGroupBuilder recurringJournalGroup = group.MapGroup("/recurringjournalentries");
        recurringJournalGroup.MapCreateRecurringJournalEntryEndpoint();
        recurringJournalGroup.MapGetRecurringJournalEntryEndpoint();
        recurringJournalGroup.MapGetRecurringJournalEntriesEndpoint();
        recurringJournalGroup.MapUpdateRecurringJournalEntryEndpoint();
        recurringJournalGroup.MapDeleteRecurringJournalEntryEndpoint();
        recurringJournalGroup.MapGenerateRecurringJournalEntryEndpoint();
        recurringJournalGroup.MapApproveRecurringJournalEntryEndpoint();

        // Retained Earnings
        // Retained Earnings
        RouteGroupBuilder retainedEarningsGroup = group.MapGroup("/retainedearnings");
        retainedEarningsGroup.MapCreateRetainedEarningsEndpoint();
        // List
        retainedEarningsGroup.MapGetRetainedEarningsEndpoint();
        // Detail
        retainedEarningsGroup.MapGetRetainedEarningsByIdEndpoint();
        // Close & Reopen
        retainedEarningsGroup.MapCloseRetainedEarningsEndpoint();
        retainedEarningsGroup.MapReopenRetainedEarningsEndpoint();
    }
}
