namespace FSH.Modules.Accounting;

/// <summary>
/// Defines all string length constants for the Accounting module using power-of-2 values.
/// 
/// **Purpose:**
/// Centralizes all string field length constraints ensuring consistency across domain entities,
/// validators, and database configurations. Uses binary-friendly sizes for optimal performance.
/// 
/// **Design Pattern:**
/// - Power-of-2 values (4, 8, 16, 32, 64, 128, 256, 512, 1024, 2048)
/// - Single source of truth for all string constraints
/// - Memory-efficient and database-optimized
/// 
/// **Naming Convention:**
/// {EntityName}{PropertyName}MaxLength or {Purpose}MaxLength
/// 
/// **Standard Sizes:**
/// - XSmall (4): Very short codes/flags
/// - Small (8): Status codes, short identifiers
/// - Regular (16): Account codes, tax codes
/// - Medium (32): Short names, types
/// - Large (64): Tenant IDs, User IDs, Reference numbers
/// - XLarge (128): Names, titles
/// - XXLarge (256): Longer names, classifications
/// - Huge (512): Descriptions
/// - XHuge (1024): Detailed descriptions
/// - XXHuge (2048): Large text content, notes
/// </summary>
public static class AccountingStringLengths
{
    // ========================================
    // Common Sizes (Reusable across entities)
    // ========================================
    
    /// <summary>XSmall: 4 characters - Very short codes, flags</summary>
    public const int XSmall = 4;
    
    /// <summary>Small: 8 characters - Status codes, currency codes</summary>
    public const int Small = 8;
    
    /// <summary>Regular: 16 characters - Account codes, reference codes</summary>
    public const int Regular = 16;
    
    /// <summary>Medium: 32 characters - Types, categories, short names</summary>
    public const int Medium = 32;
    
    /// <summary>Large: 64 characters - IDs, usernames, standard identifiers</summary>
    public const int Large = 64;
    
    /// <summary>XLarge: 128 characters - Names, titles</summary>
    public const int XLarge = 128;
    
    /// <summary>XXLarge: 256 characters - Classifications, longer names</summary>
    public const int XXLarge = 256;
    
    /// <summary>Huge: 512 characters - Descriptions, notes</summary>
    public const int Huge = 512;
    
    /// <summary>XHuge: 1024 characters - Detailed descriptions, content</summary>
    public const int XHuge = 1024;
    
    /// <summary>XXHuge: 2048 characters - Large text, detailed notes</summary>
    public const int XXHuge = 2048;

    // ========================================
    // Chart of Accounts
    // ========================================
    
    /// <summary>Chart of Account code (USOA codes like "101", "403")</summary>
    public const int AccountCode = Regular;
    
    /// <summary>Account name (e.g., "Cash and Cash Equivalents")</summary>
    public const int AccountName = XHuge;
    
    /// <summary>Account type (Asset, Liability, Equity, Revenue, Expense)</summary>
    public const int AccountType = Medium;
    
    /// <summary>USOA category classification</summary>
    public const int UsoaCategory = Regular;
    
    /// <summary>Normal balance type (Debit, Credit)</summary>
    public const int NormalBalance = Small;
    
    /// <summary>Regulatory classification</summary>
    public const int RegulatoryClassification = XXLarge;

    // ========================================
    // Journal Entries
    // ========================================
    
    /// <summary>Journal entry number/reference</summary>
    public const int JournalEntryNumber = Large;
    
    /// <summary>Journal entry type (Standard, Adjusting, Closing, Reversing)</summary>
    public const int JournalEntryType = Medium;
    
    /// <summary>Journal entry status (Draft, Posted, Approved, Rejected)</summary>
    public const int JournalEntryStatus = Medium;
    
    /// <summary>Journal entry description</summary>
    public const int JournalDescription = Huge;
    
    /// <summary>Journal entry memo/notes</summary>
    public const int JournalMemo = XXHuge;

    // ========================================
    // Invoices & Bills
    // ========================================
    
    /// <summary>Invoice/Bill number</summary>
    public const int InvoiceNumber = Large;
    
    /// <summary>Invoice/Bill status</summary>
    public const int InvoiceStatus = Medium;
    
    /// <summary>Payment terms</summary>
    public const int PaymentTerms = XXLarge;
    
    /// <summary>Line item description</summary>
    public const int LineItemDescription = Huge;

    // ========================================
    // Banking
    // ========================================
    
    /// <summary>Bank name</summary>
    public const int BankName = XXLarge;
    
    /// <summary>Bank account number</summary>
    public const int BankAccountNumber = Large;
    
    /// <summary>Routing number</summary>
    public const int RoutingNumber = Medium;
    
    /// <summary>Check number</summary>
    public const int CheckNumber = Medium;
    
    /// <summary>Check status</summary>
    public const int CheckStatus = Medium;

    // ========================================
    // Customers & Vendors
    // ========================================
    
    /// <summary>Customer/Vendor name</summary>
    public const int CustomerName = XXLarge;
    
    /// <summary>Customer/Vendor code</summary>
    public const int CustomerCode = Large;
    
    /// <summary>Contact name</summary>
    public const int ContactName = XLarge;
    
    /// <summary>Email address</summary>
    public const int Email = XXLarge;
    
    /// <summary>Phone number</summary>
    public const int PhoneNumber = Medium;
    
    /// <summary>Address line</summary>
    public const int AddressLine = XXLarge;
    
    /// <summary>City name</summary>
    public const int City = XLarge;
    
    /// <summary>State/Province code</summary>
    public const int StateCode = Small;
    
    /// <summary>Postal/ZIP code</summary>
    public const int PostalCode = Regular;
    
    /// <summary>Country code</summary>
    public const int CountryCode = Small;

    // ========================================
    // Assets
    // ========================================
    
    /// <summary>Asset name</summary>
    public const int AssetName = XXLarge;
    
    /// <summary>Asset tag/serial number</summary>
    public const int AssetTag = Large;
    
    /// <summary>Depreciation method name</summary>
    public const int DepreciationMethod = XLarge;
    
    /// <summary>Asset location</summary>
    public const int AssetLocation = XXLarge;

    // ========================================
    // Utility-Specific
    // ========================================
    
    /// <summary>Meter number</summary>
    public const int MeterNumber = Large;
    
    /// <summary>Member/Account number</summary>
    public const int MemberNumber = Large;
    
    /// <summary>Rate schedule code</summary>
    public const int RateScheduleCode = Medium;
    
    /// <summary>Rate schedule name</summary>
    public const int RateScheduleName = XXLarge;
    
    /// <summary>Power purchase agreement name</summary>
    public const int AgreementName = XXLarge;
    
    /// <summary>Regulatory report type</summary>
    public const int ReportType = XLarge;

    // ========================================
    // Common Fields
    // ========================================
    
    /// <summary>Standard name field</summary>
    public const int Name = XLarge;
    
    /// <summary>Standard description field</summary>
    public const int Description = XXHuge;
    
    /// <summary>Standard notes field</summary>
    public const int Notes = XXHuge;
    
    /// <summary>Tenant identifier</summary>
    public const int TenantId = Large;
    
    /// <summary>User identifier (created by, modified by)</summary>
    public const int UserId = Large;
    
    /// <summary>Username</summary>
    public const int UserName = XXLarge;
    
    /// <summary>Reference number</summary>
    public const int ReferenceNumber = Large;
    
    /// <summary>Tax code</summary>
    public const int TaxCode = Regular;
    
    /// <summary>Cost center code</summary>
    public const int CostCenterCode = Medium;
    
    /// <summary>Project code</summary>
    public const int ProjectCode = Large;
}
