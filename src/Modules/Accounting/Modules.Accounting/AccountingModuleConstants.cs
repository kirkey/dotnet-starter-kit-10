namespace FSH.Modules.Accounting;

/// <summary>
/// Contains module-level constants for the Accounting module.
/// </summary>
public static class AccountingModuleConstants
{
    /// <summary>
    /// The module name.
    /// </summary>
    public const string ModuleName = "Accounting";

    /// <summary>
    /// The module description.
    /// </summary>
    public const string ModuleDescription = "Comprehensive accounting module for managing financial operations, including chart of accounts, general ledger, invoices, bills, payments, and financial reporting.";

    /// <summary>
    /// The database schema name for the Accounting module.
    /// </summary>
    public const string SchemaName = "accounting";

    /// <summary>
    /// The API route prefix for the Accounting module.
    /// </summary>
    public const string RoutePrefix = "api/v{version:apiVersion}/accounting";

    /// <summary>
    /// The API tag name for Accounting module endpoints.
    /// </summary>
    public const string ApiTag = "Accounting";
}
