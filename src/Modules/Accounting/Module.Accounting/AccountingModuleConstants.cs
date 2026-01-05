namespace FSH.Module.Accounting;

/// <summary>
/// Constants used throughout the Accounting module for consistency.
/// 
/// Used for:
/// - Database schema naming
/// - API route prefixes
/// - Configuration keys
/// </summary>
public static class AccountingModuleConstants
{
    /// <summary>
    /// Database schema name for all Accounting-related tables.
    /// </summary>
    public const string SchemaName = "accounting";
    
    /// <summary>
    /// Route prefix used for Accounting API endpoints (e.g., /api/v1/accounting).
    /// </summary>
    public const string Prefix = "accounting";
}
