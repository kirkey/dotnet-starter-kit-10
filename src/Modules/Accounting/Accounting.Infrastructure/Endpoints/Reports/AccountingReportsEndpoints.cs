using Accounting.Infrastructure.Endpoints.Reports.v1;
using Carter;

namespace Accounting.Infrastructure.Endpoints.Reports;

/// <summary>
/// Carter module for Accounting PDF Report endpoints.
/// Groups all QuestPDF report generation endpoints under /api/v1/accounting/reports.
/// </summary>
public class AccountingReportsEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/accounting")
            .WithTags("Accounting Reports");

        group.MapGenerateAccountingGeneralLedgerReportEndpoint();
        group.MapGenerateAccountingJournalEntryReportEndpoint();
        group.MapGenerateAccountingAgedReceivablesReportEndpoint();
        group.MapGenerateAccountingAgedPayablesReportEndpoint();
        group.MapGenerateAccountingTrialBalanceReportEndpoint();
        
        // Financial Statement Reports
        group.MapGenerateAccountingBalanceSheetReportEndpoint();
        group.MapGenerateAccountingIncomeStatementReportEndpoint();
        group.MapGenerateAccountingCashFlowStatementReportEndpoint();
        
        // Transactional Document Reports
        group.MapGenerateAccountingInvoiceReportEndpoint();
        group.MapGenerateAccountingCreditMemoReportEndpoint();
        group.MapGenerateAccountingBillReportEndpoint();
        group.MapGenerateAccountingDebitMemoReportEndpoint();
        group.MapGenerateAccountingPaymentReportEndpoint();
        group.MapGenerateAccountingCheckReportEndpoint();
        
        // Customer/Vendor Statement Reports
        group.MapGenerateAccountingCustomerStatementReportEndpoint();
        group.MapGenerateAccountingVendorStatementReportEndpoint();
        
        // Banking Reports
        group.MapGenerateAccountingBankReconciliationReportEndpoint();
        group.MapGenerateAccountingCheckRegisterReportEndpoint();
        
        // Asset Reports
        group.MapGenerateAccountingFixedAssetRegisterReportEndpoint();
        group.MapGenerateAccountingDepreciationScheduleReportEndpoint();
        
        // Budget & Analysis Reports
        group.MapGenerateAccountingBudgetVsActualReportEndpoint();
        group.MapGenerateAccountingVarianceAnalysisReportEndpoint();
        group.MapGenerateAccountingProjectCostReportEndpoint();
        
        // Accrual/Deferral Reports
        group.MapGenerateAccountingPrepaidExpenseReportEndpoint();
        group.MapGenerateAccountingDeferredRevenueReportEndpoint();
        
        // Summary Reports
        group.MapGenerateAccountingChartOfAccountsStructureReportEndpoint();
        group.MapGenerateAccountingWriteOffSummaryReportEndpoint();
        group.MapGenerateAccountingBankAccountSummaryReportEndpoint();
        group.MapGenerateAccountingPeriodStatusReportEndpoint();
        group.MapGenerateAccountingFiscalPeriodCloseReportEndpoint();
    }
}
