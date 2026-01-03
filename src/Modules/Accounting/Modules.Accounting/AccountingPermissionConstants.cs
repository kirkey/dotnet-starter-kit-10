namespace FSH.Modules.Accounting;

/// <summary>
/// Defines all permission constants for the Accounting module.
/// 
/// Permissions follow the pattern: {ModuleName}.{Action}
/// 
/// For example:
/// - Accounting.View: Permission to view accounting records
/// - Accounting.Create: Permission to create new accounting transactions
/// - Accounting.Update: Permission to modify accounting records
/// - Accounting.Delete: Permission to delete accounting records
/// 
/// These permissions are registered with the framework and can be assigned to roles
/// and used for authorization checks on endpoints and operations.
/// </summary>
public static class AccountingPermissionConstants
{
    private const string Module = "Accounting";

    public static class ChartOfAccounts
    {
        public const string View = $"{Module}.ChartOfAccounts.View";
        public const string Search = $"{Module}.ChartOfAccounts.Search";
        public const string Create = $"{Module}.ChartOfAccounts.Create";
        public const string Update = $"{Module}.ChartOfAccounts.Update";
        public const string Delete = $"{Module}.ChartOfAccounts.Delete";
    }

    public static class GeneralLedger
    {
        public const string View = $"{Module}.GeneralLedger.View";
        public const string Search = $"{Module}.GeneralLedger.Search";
        public const string Create = $"{Module}.GeneralLedger.Create";
        public const string Update = $"{Module}.GeneralLedger.Update";
        public const string Delete = $"{Module}.GeneralLedger.Delete";
        public const string Post = $"{Module}.GeneralLedger.Post";
    }

    public static class Invoices
    {
        public const string View = $"{Module}.Invoices.View";
        public const string Search = $"{Module}.Invoices.Search";
        public const string Create = $"{Module}.Invoices.Create";
        public const string Update = $"{Module}.Invoices.Update";
        public const string Delete = $"{Module}.Invoices.Delete";
        public const string Post = $"{Module}.Invoices.Post";
    }

    public static class Bills
    {
        public const string View = $"{Module}.Bills.View";
        public const string Search = $"{Module}.Bills.Search";
        public const string Create = $"{Module}.Bills.Create";
        public const string Update = $"{Module}.Bills.Update";
        public const string Delete = $"{Module}.Bills.Delete";
        public const string Post = $"{Module}.Bills.Post";
    }

    public static class Payments
    {
        public const string View = $"{Module}.Payments.View";
        public const string Search = $"{Module}.Payments.Search";
        public const string Create = $"{Module}.Payments.Create";
        public const string Update = $"{Module}.Payments.Update";
        public const string Delete = $"{Module}.Payments.Delete";
        public const string Post = $"{Module}.Payments.Post";
    }

    public static class AccountsPayable
    {
        public const string View = $"{Module}.AccountsPayable.View";
        public const string Search = $"{Module}.AccountsPayable.Search";
        public const string Create = $"{Module}.AccountsPayable.Create";
        public const string Update = $"{Module}.AccountsPayable.Update";
        public const string Delete = $"{Module}.AccountsPayable.Delete";
    }

    public static class AccountsReceivable
    {
        public const string View = $"{Module}.AccountsReceivable.View";
        public const string Search = $"{Module}.AccountsReceivable.Search";
        public const string Create = $"{Module}.AccountsReceivable.Create";
        public const string Update = $"{Module}.AccountsReceivable.Update";
        public const string Delete = $"{Module}.AccountsReceivable.Delete";
    }

    public static class Banks
    {
        public const string View = $"{Module}.Banks.View";
        public const string Search = $"{Module}.Banks.Search";
        public const string Create = $"{Module}.Banks.Create";
        public const string Update = $"{Module}.Banks.Update";
        public const string Delete = $"{Module}.Banks.Delete";
        public const string Reconcile = $"{Module}.Banks.Reconcile";
    }

    public static class FixedAssets
    {
        public const string View = $"{Module}.FixedAssets.View";
        public const string Search = $"{Module}.FixedAssets.Search";
        public const string Create = $"{Module}.FixedAssets.Create";
        public const string Update = $"{Module}.FixedAssets.Update";
        public const string Delete = $"{Module}.FixedAssets.Delete";
        public const string Depreciate = $"{Module}.FixedAssets.Depreciate";
    }

    public static class Budgets
    {
        public const string View = $"{Module}.Budgets.View";
        public const string Search = $"{Module}.Budgets.Search";
        public const string Create = $"{Module}.Budgets.Create";
        public const string Update = $"{Module}.Budgets.Update";
        public const string Delete = $"{Module}.Budgets.Delete";
    }

    public static class JournalEntries
    {
        public const string View = $"{Module}.JournalEntries.View";
        public const string Search = $"{Module}.JournalEntries.Search";
        public const string Create = $"{Module}.JournalEntries.Create";
        public const string Update = $"{Module}.JournalEntries.Update";
        public const string Delete = $"{Module}.JournalEntries.Delete";
        public const string Post = $"{Module}.JournalEntries.Post";
    }

    public static class AccountingPeriods
    {
        public const string View = $"{Module}.AccountingPeriods.View";
        public const string Search = $"{Module}.AccountingPeriods.Search";
        public const string Create = $"{Module}.AccountingPeriods.Create";
        public const string Update = $"{Module}.AccountingPeriods.Update";
        public const string Delete = $"{Module}.AccountingPeriods.Delete";
        public const string Close = $"{Module}.AccountingPeriods.Close";
    }

    public static class CostCenters
    {
        public const string View = $"{Module}.CostCenters.View";
        public const string Search = $"{Module}.CostCenters.Search";
        public const string Create = $"{Module}.CostCenters.Create";
        public const string Update = $"{Module}.CostCenters.Update";
        public const string Delete = $"{Module}.CostCenters.Delete";
    }

    public static class Reports
    {
        public const string View = $"{Module}.Reports.View";
        public const string TrialBalance = $"{Module}.Reports.TrialBalance";
        public const string IncomeStatement = $"{Module}.Reports.IncomeStatement";
        public const string BalanceSheet = $"{Module}.Reports.BalanceSheet";
        public const string CashFlow = $"{Module}.Reports.CashFlow";
    }

    /// <summary>
    /// Returns all permissions defined in this module.
    /// This method is called during module registration to register all permissions with the framework.
    /// </summary>
    public static IReadOnlyList<string> GetPermissions()
    {
        return new List<string>
        {
            // Chart of Accounts
            ChartOfAccounts.View,
            ChartOfAccounts.Search,
            ChartOfAccounts.Create,
            ChartOfAccounts.Update,
            ChartOfAccounts.Delete,

            // General Ledger
            GeneralLedger.View,
            GeneralLedger.Search,
            GeneralLedger.Create,
            GeneralLedger.Update,
            GeneralLedger.Delete,
            GeneralLedger.Post,

            // Invoices
            Invoices.View,
            Invoices.Search,
            Invoices.Create,
            Invoices.Update,
            Invoices.Delete,
            Invoices.Post,

            // Bills
            Bills.View,
            Bills.Search,
            Bills.Create,
            Bills.Update,
            Bills.Delete,
            Bills.Post,

            // Payments
            Payments.View,
            Payments.Search,
            Payments.Create,
            Payments.Update,
            Payments.Delete,
            Payments.Post,

            // Accounts Payable
            AccountsPayable.View,
            AccountsPayable.Search,
            AccountsPayable.Create,
            AccountsPayable.Update,
            AccountsPayable.Delete,

            // Accounts Receivable
            AccountsReceivable.View,
            AccountsReceivable.Search,
            AccountsReceivable.Create,
            AccountsReceivable.Update,
            AccountsReceivable.Delete,

            // Banks
            Banks.View,
            Banks.Search,
            Banks.Create,
            Banks.Update,
            Banks.Delete,
            Banks.Reconcile,

            // Fixed Assets
            FixedAssets.View,
            FixedAssets.Search,
            FixedAssets.Create,
            FixedAssets.Update,
            FixedAssets.Delete,
            FixedAssets.Depreciate,

            // Budgets
            Budgets.View,
            Budgets.Search,
            Budgets.Create,
            Budgets.Update,
            Budgets.Delete,

            // Journal Entries
            JournalEntries.View,
            JournalEntries.Search,
            JournalEntries.Create,
            JournalEntries.Update,
            JournalEntries.Delete,
            JournalEntries.Post,

            // Accounting Periods
            AccountingPeriods.View,
            AccountingPeriods.Search,
            AccountingPeriods.Create,
            AccountingPeriods.Update,
            AccountingPeriods.Delete,
            AccountingPeriods.Close,

            // Cost Centers
            CostCenters.View,
            CostCenters.Search,
            CostCenters.Create,
            CostCenters.Update,
            CostCenters.Delete,

            // Reports
            Reports.View,
            Reports.TrialBalance,
            Reports.IncomeStatement,
            Reports.BalanceSheet,
            Reports.CashFlow
        };
    }
}
